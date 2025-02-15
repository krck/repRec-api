<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Http\Requests\StoreUserRequest;
use App\Http\Requests\UpdateUserRequest;
use App\Models\User;

class UserController extends Controller
{
    // GET all users
    // (Admin only)
    public function index()
    {
        $users = User::with('roles')->get();
        return response()->json($users);
    }

    // GET my user (Authenticated user)
    public function myUser(Request $request)
    {
        $userId = $request->user()->id; // Assume the authenticated user is available in the request
        $user = User::with('roles')->findOrFail($userId);
        return response()->json($user);
    }
}
