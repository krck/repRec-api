<?php

namespace App\Http\Controllers;

use Illuminate\Http\JsonResponse;

class VersionController extends Controller
{
    /**
     * GET api version
     */
    public function showVersion(): JsonResponse
    {
        $version = '0.0.99'; // Set the API version here
        return response()->json(['version' => $version]);
    }
}
