<?php

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\VersionController;
use App\Http\Controllers\RolesController;

Route::get('/version', [VersionController::class, 'showVersion']);

Route::get('/roles', [RolesController::class, 'index']);
Route::get('/roles/{id}', [RolesController::class, 'show']);
