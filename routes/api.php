<?php

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\LogController;
use App\Http\Controllers\RoleController;
use App\Http\Controllers\UserController;
use App\Http\Controllers\VersionController;
use App\Http\Middleware\AuthenticationMiddleware;

Auth::shouldUse('auth0-api');

// Endpoints without Authentication
Route::get('/version', [VersionController::class, 'showVersion'])->withoutMiddleware(['middleware' => 'auth']);

// Endpoints with Authentication
Route::group(['middleware' => 'auth'], function () {
    // Log
    Route::get('/logs/{filterType}', [LogController::class, 'index']);
    // Role
    Route::get('/roles', [RoleController::class, 'index']);
    Route::get('/roles/{id}', [RoleController::class, 'show']);
    // User
    Route::get('/users', [UserController::class, 'index']);
    Route::post('/users', [UserController::class, 'store']);
    Route::put('/users/settings/{id}', [UserController::class, 'updateSettings']);
});
