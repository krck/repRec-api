<?php

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\RolesController;
use App\Http\Controllers\VersionController;
use App\Http\Middleware\AuthenticationMiddleware;

Auth::shouldUse('auth0-api');

// Endpoints without Authentication
Route::get('/version', [VersionController::class, 'showVersion'])->withoutMiddleware(['middleware' => 'auth']);

// Endpoints with Authentication
Route::group(['middleware' => 'auth'], function () {
    Route::get('/roles', [RolesController::class, 'index']);
    Route::get('/roles/{id}', [RolesController::class, 'show']);
});
