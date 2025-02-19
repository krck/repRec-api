<?php

use Illuminate\Foundation\Application;
use App\Http\Middleware\SecurityHeaderMiddleware;
use Illuminate\Foundation\Configuration\Exceptions;
use Illuminate\Foundation\Configuration\Middleware;

return Application::configure(basePath: dirname(__DIR__))
    ->withRouting(
        web: __DIR__ . '/../routes/web.php',
        api: __DIR__ . '/../routes/api.php',
        commands: __DIR__ . '/../routes/console.php',
        health: '/up',
    )
    ->withMiddleware(function (Middleware $middleware) {
        $middleware->api('auth', 'App\Http\Middleware\AuthenticationMiddleware');
        $middleware->append(SecurityHeaderMiddleware::class);
    })
    ->withExceptions(function (Exceptions $exceptions) {
        // Create a custom context for the exceptions
        // Extract: Exception Type (name), File + Line and user from request
        $exceptions->context(function ($e) {
            $request = Request::instance();
            $authUser = $request->attributes->get('auth0_user');
            return [
                'type' => $e instanceof Throwable ? get_class($e) : null,
                'file' => $e instanceof Throwable ? $e->getFile() : null,
                'line' => $e instanceof Throwable ? strval($e->getLine()) : null,
                'user' => $authUser != null ? $authUser['id'] : null
            ];
        });

        // Automatic De-duplication, in case the same exception object is reported multiple times
        $exceptions->dontReportDuplicates();
    })->create();
