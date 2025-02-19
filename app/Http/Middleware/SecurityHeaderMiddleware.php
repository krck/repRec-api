<?php

namespace App\Http\Middleware;

use Closure;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Log;
use Symfony\Component\HttpFoundation\Response;

class SecurityHeaderMiddleware
{
    private readonly array $headersToAdd;
    private readonly array $headersToRemove;

    /**
     * Constructor.
     */
    public function __construct()
    {
        $this->headersToAdd = $this->getAllSecurityHeadersToAdd();
        $this->headersToRemove = $this->getAllSecurityHeadersToRemove();
    }

    /** 
     * Global Security Header Middleware
     * https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers#security 
     */
    public function handle(Request $request, Closure $next): Response
    {
        /** @var Response $response */
        $response = $next($request);
        try {
            // Update the API Response Headers
            foreach ($this->headersToAdd as $key => $value) {
                $response->headers->set($key, $value, true); // Replace existing => true
            }
            foreach ($this->headersToRemove as $key) {
                $response->headers->remove($key);
            }

            return $response;
        } catch (\Throwable $th) {
            // Middleware Exceptions: Log immediately and abort/fail with a "clean" response
            Log::error("SecurityHeaderMiddleware: " . $th->getMessage());
            abort(Response::HTTP_INTERNAL_SERVER_ERROR);
        }
    }

    #region PRIVATE FUNCTIONS

    private function getAllSecurityHeadersToAdd(): array
    {
        return array_merge(
            // Simple/Hardcoded Headers, that will (most likely) never change
            [
                'X-Content-Type-Options' => 'nosniff',
                'X-Frame-Options' => 'SAMEORIGIN',
                'X-XSS-Protection' => '1; mode=block',
                'X-Permitted-Cross-Domain-Policies' => 'none',
                'Referrer-Policy' => 'no-referrer-when-downgrade',
                'Permissions-Policy' => 'geolocation=(self), microphone=(), camera=(), payment=()',
            ],
            // Complex/Variable Headers, that might change (Based on ENV, based on Frontend)
            [
                'Cross-Origin-Resource-Policy' => 'same-origin', // DEV: 'cross-origin'
                'Cross-Origin-Embedder-Policy' => 'require-corp', // DEV: 'unsafe-none'
                'Cross-Origin-Opener-Policy' => 'same-origin', // DEV: 'unsafe-none'
                // DEV: Might allow 'unsafe-inline' on script, style, etc.
                'Content-Security-Policy' => trim(
                    "default-src 'self'; " .
                        "script-src 'self'; " .
                        "style-src 'self' 'unsafe-inline'; " .
                        "object-src 'none'; " .
                        "frame-ancestors 'none'; "
                ),
                // Cache control (prevent browsers from storing data)
                'Cache-Control' => 'no-store, no-cache, must-revalidate, max-age=0',
                'Pragma' => 'no-cache',
            ],
            // Production ENV only Headers
            (app()->environment('production') ? [
                // HSTS (Forces HTTPS)
                'Strict-Transport-Security' => 'max-age=31536000; includeSubDomains; preload',
            ] : [])
        );
    }

    private function getAllSecurityHeadersToRemove(): array
    {
        return [
            // X-Powered-By must be removed in the php.ini
            // (find the ini "> php --ini" and set "expose_php = Off")
            'X-Powered-By',
            'X-Developed-By'
        ];
    }

    #endregion
}
