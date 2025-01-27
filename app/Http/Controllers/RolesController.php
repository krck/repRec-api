<?php

namespace App\Http\Controllers;

use App\Models\Roles;
use Illuminate\Http\JsonResponse;
use App\Http\Requests\StoreRolesRequest;
use App\Http\Requests\UpdateRolesRequest;

class RolesController extends Controller
{
    // Get all roles
    public function index(): JsonResponse
    {
        $roles = Roles::all();
        return response()->json($roles);
    }

    // Get role by id
    public function show($id): JsonResponse
    {
        $role = Roles::find($id);
        return response()->json($role);
    }
}
