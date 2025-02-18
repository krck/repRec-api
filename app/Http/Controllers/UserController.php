<?php

namespace App\Http\Controllers;

use App\Models\Role;
use App\Models\User;
use App\Exceptions\ApiException;
use Illuminate\Http\JsonResponse;
use App\Http\Requests\StoreUserRequest;
use App\Http\Requests\UpdateUserRequest;
use Symfony\Component\HttpFoundation\Response;

class UserController extends Controller
{
    /**
     * GET all users
     * (Admin only)
     */
    public function index(): JsonResponse
    {
        $users = User::with('roles')->get();
        return response()->json($users);
    }

    /**
     * POST my user
     * (Create if not exists)
     */
    public function store(StoreUserRequest $request): JsonResponse
    {
        // Get user ID from auth token (Auth0 ID)
        $authUser = $request->attributes->get('auth0_user');
        $authUserId = $authUser['id'];

        // Get validated user data from the request Body
        $validUserData = $request->validated();
        if (empty($authUserId) || $authUserId != $validUserData['id'])
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

    /**
     * PUT update user settings
     * (only allowed for the authenticated user)
     */
    public function updateSettings(UpdateUserRequest $request, $id): JsonResponse
    {
        // Get user ID from auth token (Auth0 ID)
        $authUser = $request->attributes->get('auth0_user');
        $authUserId = $authUser['id'];
        if (empty($authUserId) || $authUserId != $id)
            throw new ApiException("User Id mismatch", Response::HTTP_UNPROCESSABLE_ENTITY);

        // Get the validated data from the request body, and the user to-update from the DB 
        $validatedSettings = $request->validated();
        $user = User::findOrFail($authUserId);

        // Update the user and return the result
        $user->update($validatedSettings);
        return response()->json($user);
    }
}
