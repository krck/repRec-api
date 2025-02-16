<?php

namespace App\Http\Middleware;

use Closure;
use Firebase\JWT\JWT;
use Firebase\JWT\Key;
use Illuminate\Http\Request;
use Symfony\Component\HttpFoundation\Response;

class AuthUserMiddleware
{
    /**
     * Handle an incoming request.
     *
     * @param  \Closure(\Illuminate\Http\Request): (\Symfony\Component\HttpFoundation\Response)  $next
     */
    public function handle(Request $request, Closure $next): Response
    {
        try {
            $jwt = $request->bearerToken();
            if (!$jwt) {
                return response()->json(['error' => 'Unauthorized'], Response::HTTP_UNAUTHORIZED);
            }

            if (substr($jwt, 0, 7) === 'Bearer ') {
                $jwt = substr($jwt, 7);
            }

            $tokenParts = explode(".", $jwt);
            $tokenHeader = base64_decode($tokenParts[0]);
            $tokenPayload = base64_decode($tokenParts[1]);

            // Extract user data from the token
            $decodedPayload = json_decode($tokenPayload);
            if (json_last_error() === JSON_ERROR_NONE) {
                $auth0User = [
                    'id' => $decodedPayload->sub ?? '', // Auth0 user ID
                    'email' => $decodedPayload->email ?? null,
                    'email_verified' => $decodedPayload->email_verified ?? false,
                ];

                // Add user data to the request object
                $request->merge(['auth0_user' => $auth0User]);
            } else {
                // Handle invalid JSON
            }


            return $next($request);
        } catch (\Exception $e) {
            return response()->json(['error' => 'Invalid token'], Response::HTTP_UNAUTHORIZED);
        }
    }
}
