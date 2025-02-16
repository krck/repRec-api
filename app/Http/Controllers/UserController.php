<?php

namespace App\Http\Controllers;

use App\Models\Role;
use App\Models\User;
use Illuminate\Http\Request;
use App\Exceptions\ApiException;
use App\Http\Requests\StoreUserRequest;
use Symfony\Component\HttpFoundation\Response;

class UserController extends Controller
{
    // GET all users
    // (Admin only)
    public function index()
    {
        $users = User::with('roles')->get();
        return response()->json($users);
    }

    // POST my user (Create if not exists)
    public function store(Request $request)
    {
        // Get user ID from auth token (Auth0 ID)
        $authUser = $request->attributes->get('auth0_user');
        $auth0Id = $authUser['id'];

        // Get validated user data from the request Body
        // $validUserData = $request->validated();
        $validUserData = $request->validate([
            'id' => 'required|string|unique:users,id',
            'email' => 'required|email|unique:users,email',
            'email_verified' => 'boolean',
            'nickname' => 'nullable|string',
            'setting_timezone' => 'required|string',
            'setting_weight_unit' => 'required|string',
            'setting_distance_unit' => 'required|string',
        ]);

        if (empty($auth0Id) || $auth0Id != $validUserData['id'])
            throw new ApiException("User Id mismatch", Response::HTTP_UNPROCESSABLE_ENTITY);

        // If the user does not exist already, create a new record
        $dbUser = User::firstOrCreate(
            ['id' => $validUserData['id']],
            $validUserData
        );

        // Assign default roles (User, Planner)
        if ($dbUser->wasRecentlyCreated) {
            $defaultRoles = Role::whereIn('name', ['User', 'Planner'])->pluck('id');
            $dbUser->roles()->attach($defaultRoles);
        } else {
            // If the user existed already, but the Email or Email-Validated changed, update the user
            if ($dbUser->email != $validUserData['email'] || $dbUser->email_verified != $validUserData['email_verified']) {
                $dbUser->update([
                    'email' => $validUserData['email'],
                    'email_verified' => $validUserData['email_verified'],
                ]);
            }
        }

        return response()->json($dbUser, 201);
    }

    // PUT update user settings (only allowed for the authenticated user)
    public function updateSettings(Request $request)
    {
        $userId = $request->user()->id;
        $user = User::findOrFail($userId);

        $validatedData = $request->validate([
            'setting_timezone' => 'string|required',
            'setting_weight_unit' => 'string|required',
            'setting_distance_unit' => 'string|required',
        ]);

        $user->update($validatedData);
        return response()->json($user);
    }
}
