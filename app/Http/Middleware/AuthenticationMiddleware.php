<?php

namespace App\Http\Middleware;

use Closure;
use Illuminate\Http\Request;
use App\Exceptions\ApiException;
use Symfony\Component\HttpFoundation\Response;
use Illuminate\Auth\Middleware\Authenticate as Middleware;

class AuthenticationMiddleware extends Middleware
{
    /**
     * Handle an incoming request.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  \Closure  $next
     * @param  string  ...$guards
     * @return Response
     */
    public function handle($request, Closure $next, ...$guards): Response
    {
        $jwt = $request->bearerToken();
        if (!$jwt) {
            throw new ApiException('Unauthorized', Response::HTTP_UNAUTHORIZED);
        }

        if (substr($jwt, 0, 7) === 'Bearer ') {
            $jwt = substr($jwt, 7);
        }
        $tokenParts = explode(".", $jwt);
        $tokenHeader = base64_decode($tokenParts[0]);
        $tokenPayload = base64_decode($tokenParts[1]);

        // Extract user data from the token
        // And add user data to the request query object
        // (so that every endpoint can access parsed user data via the $request)
        $decodedPayload = json_decode($tokenPayload);
        if (json_last_error() === JSON_ERROR_NONE) {
            $auth0User = [
                'id' => $decodedPayload->sub ?? '', // "sub" = Auth0 User ID
                'email' => $decodedPayload->email ?? null,
                'email_verified' => $decodedPayload->email_verified ?? false,
            ];

            $request->attributes->set('auth0_user', $auth0User);
        } else {
            throw new ApiException('Invalid token', Response::HTTP_UNAUTHORIZED);
        }

        return $next($request);
    }

    /**
     * Laravel's default behavior in case of auth exceptions is to redirect to a login page
     * Since this is an API only, just throw an exception instead of redirecting (no login)
     * (overwrite default "Authenticate" middleware behavior)
     */
    protected function redirectTo(Request $request): ?string
    {
        if ($this->auth->guard()->guest())
            throw new ApiException('Unauthorized', Response::HTTP_UNAUTHORIZED);

        return null;
    }
}
