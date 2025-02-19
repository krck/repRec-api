<?php

use App\Common\Enums\EnumRole;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\LogController;
use App\Http\Controllers\RoleController;
use App\Http\Controllers\UserController;
use App\Http\Controllers\VersionController;
use App\Http\Middleware\RoleAccessMiddleware as Roles;

Auth::shouldUse('auth0-api');

// Endpoints without Authentication
Route::get('/version', [VersionController::class, 'showVersion'])->withoutMiddleware(['middleware' => 'auth']);

// Endpoints with Authentication
Route::group(['middleware' => 'auth'], function () {
    // Log
    Route::get('/logs/{filterType}', [LogController::class, 'index'])->middleware(Roles::class . ':admin,user');;
    // Role
    Route::get('/roles', [RoleController::class, 'index']);
    Route::get('/roles/{id}', [RoleController::class, 'show']);
    // User
    Route::get('/users', [UserController::class, 'index']);
    Route::post('/users', [UserController::class, 'store']);
    Route::put('/users/settings/{id}', [UserController::class, 'updateSettings']);
});
