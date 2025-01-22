<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;

class VersionController extends Controller
{
    public function showVersion()
    {
        $version = '0.0.1'; // Set the API version here
        return response()->json(['version' => $version]);
    }
}
