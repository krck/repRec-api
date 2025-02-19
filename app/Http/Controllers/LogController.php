<?php

namespace App\Http\Controllers;

use App\Models\Log;
use Illuminate\Http\Request;
use App\Exceptions\ApiException;
use Illuminate\Http\JsonResponse;
use Symfony\Component\HttpFoundation\Response;

class LogController extends Controller
{
    /**
     * GET all error logs with their assigned users
     * (Admin only)
     */
    public function index(Request $request, $filterType): JsonResponse
    {
        // Validate filterType parameter
        if (!is_numeric($filterType) || $filterType < 1 || $filterType > 3)
            throw new ApiException("FilterType unknown", Response::HTTP_UNPROCESSABLE_ENTITY);

        // Generate a filterDate in the past
        $filterDate = (
            ($filterType == 1 ? now()->subHour()                        // 1 = Last Hour
                : ($filterType == 2 ? now()->subDay()                   // 2 = Last Day
                    : ($filterType == 3 ? now()->subWeek() : now())))   // 3 = Last Week
        );

        // Get all Log data based on the filterValue past date
        // (Left join users - there is no FK connection, but the name is needed)
        $logs = Log::leftJoin('users', 'logs.user_id', '=', 'users.id')
            ->where('logs.timestamp', '>=', $filterDate)
            ->orderByDesc('logs.timestamp')
            ->get([
                'logs.timestamp',
                'log_level as level',
                'users.nickname as userName',
                'logs.exception_type as exceptionType',
                'logs.message',
                'logs.source',
            ]);

        // Error log return only incudes specific columns
        return response()->json($logs);
    }
}
