<?php

namespace App\Http\Middleware;

use Closure;
use Exception;
use Illuminate\Http\Request;
use App\Services\UserService;
use App\Common\Enums\EnumRole;
use Illuminate\Support\Facades\Log;
use Symfony\Component\HttpFoundation\Response;

class RoleAccessMiddleware
{
    /**
     * Authorization: Check if user has the specified roles to access the endpoint
     */
    public function handle(Request $request, Closure $next, ...$roles): Response
    {
        $authUserId = null;
        try {
            // Get user ID from auth token (Auth0 ID)
            $authUser = $request->attributes->get('auth0_user');
            $authUserId = $authUser['id'];
            if ($authUserId == null || empty($authUserId))
                throw new Exception("No valid user");

            // Load user roles (from cache or db) and verify that at least one matches the filter
            $userService = app(UserService::class);
            $userRolesHash = $userService->getUserRoles($authUserId);
            foreach ($roles as $role) {
                if (($role == "admin" && $userRolesHash[EnumRole::Admin] == true)
                    || ($role == "planner" && $userRolesHash[EnumRole::Planner] == true)
                    || ($role == "user" && $userRolesHash[EnumRole::User] == true)
                ) {
                    return $next($request);
                }
            }

            // If none matches, deny further execution (no next)
            throw new Exception("No valid role");
        } catch (\Throwable $th) {
            // Middleware Exceptions: Log immediately and abort/fail with a "clean" response
            $userStr = ($authUserId != null ? " User - $authUserId " : "");
            Log::error("RoleAccessMiddleware: " . $userStr . $th->getMessage());
            abort(Response::HTTP_FORBIDDEN);
        }
    }
}
