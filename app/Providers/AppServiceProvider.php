<?php

namespace App\Providers;

use Route;
use App\Services\UserService;
use Illuminate\Support\Facades\URL;
use Illuminate\Support\ServiceProvider;

class AppServiceProvider extends ServiceProvider
{
    protected $namespace = 'App\Http\Controllers';

    /**
     * Register any application services.
     */
    public function register(): void
    {
        // Global UserService to store/retrieve user-roles
        $this->app->singleton(UserService::class, function () {
            return new UserService();
        });
    }

    /**
     * Bootstrap any application services.
     */
    public function boot(): void
    {
        // Force HTTPS for all routes on PROD
        if (app()->environment('production')) {
            URL::forceScheme('https');
        }
    }

    protected function mapApiRoutes()
    {
        Route::prefix('api')
            ->middleware('api')
            ->namespace($this->namespace)
            ->group(base_path('routes/api.php'));
    }
}
