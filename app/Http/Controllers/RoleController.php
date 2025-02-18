<?php

namespace App\Http\Controllers;

use App\Models\Role;
use Illuminate\Http\JsonResponse;

class RoleController extends Controller
{
    /**
     * GET all roles
     */
    public function index(): JsonResponse
    {
        $roles = Role::all();
        return response()->json($roles);
    }

    /**
     * GET role by id
     */
    public function show($id): JsonResponse
    {
        $role = Role::find($id);
        return response()->json($role);
    }
}
