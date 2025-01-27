<?php

namespace App\Http\Middleware;

use App\Exceptions\ApiException;
use Illuminate\Auth\Middleware\Authenticate as Middleware;
use Illuminate\Http\Request;

class Authenticate extends Middleware
{
    /**
     * Laravel's default behavior in case of auth exceptions is to redirect to a login page
     * Since this is an API only, just throw an exception instead of redirecting (no login)
     * (overwrite default "Authenticate" middleware behavior)
     */
    protected function redirectTo(Request $request): ?string
    {
        if ($this->auth->guard()->guest())
            throw new ApiException('Unauthorized', 401);

        return null;
    }
}
