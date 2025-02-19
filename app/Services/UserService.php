<?php

namespace App\Services;

use Illuminate\Support\Facades\Cache;
use Illuminate\Support\Facades\DB;

class UserService
{
    protected const CACHE_KEY = 'user_roles_';
    protected const CACHE_TTL = 1800;       // Absolute expiration in seconds (30 minutes)
    protected const CACHE_SLIDING = 600;    // Sliding expiration (10 minutes)

    /**
     * Load the users roles from cache or database
     * (userId is the Auth0 ID, combined with a cache key)
     */
    public function getUserRoles(string $userId): array
    {
        $cacheKey = self::CACHE_KEY . $userId;
        $userRoles = Cache::remember($cacheKey, self::CACHE_TTL, function () use ($userId) {
            return DB::table('user_roles')
                ->where('user_id', $userId)
                ->pluck('role_id')
                ->toArray();
        });

        $userRolesHash = [];
        foreach ($userRoles as $value) {
            if (!array_key_exists($value, $userRolesHash)) {
                echo $value . " ";
                $userRolesHash[$value] = true;
            }
        }

        return $userRolesHash;
    }

    /**
     * Clear users roles from the cache
     * (userId is the Auth0 ID, combined with a cache key)
     */
    public function clearUserCache(int $userId)
    {
        Cache::forget(self::CACHE_KEY . $userId);
    }
}
