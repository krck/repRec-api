<?php

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\RoleController;
use App\Http\Controllers\UserController;
use App\Http\Controllers\VersionController;
use App\Http\Middleware\AuthenticationMiddleware;

Auth::shouldUse('auth0-api');

// Endpoints without Authentication
Route::get('/version', [VersionController::class, 'showVersion'])->withoutMiddleware(['middleware' => 'auth']);

// Endpoints with Authentication
Route::group(['middleware' => 'auth'], function () {
    // Role
    Route::get('/roles', [RoleController::class, 'index']);
    Route::get('/roles/{id}', [RoleController::class, 'show']);
    // User
    Route::get('/users', [UserController::class, 'index']);
    Route::get('/users/{id}', [UserController::class, 'myUser']);
});
