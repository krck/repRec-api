<?php

namespace App\Http\Controllers;

use App\Models\Role;
use Illuminate\Http\JsonResponse;
use App\Http\Requests\StoreRoleRequest;
use App\Http\Requests\UpdateRoleRequest;

class RoleController extends Controller
{
    // Get all roles
    public function index(): JsonResponse
    {
        $roles = Role::all();
        return response()->json($roles);
    }

    // Get role by id
    public function show($id): JsonResponse
    {
        $role = Role::find($id);
        return response()->json($role);
    }
}
