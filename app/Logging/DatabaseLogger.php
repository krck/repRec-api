<?php

namespace App\Logging;

use Monolog\Handler\AbstractProcessingHandler;
use Illuminate\Support\Facades\Log;
use Monolog\Logger;
use DB;

class DatabaseLogger
{
    /**
     * Create a custom Monolog instance for (Postgres) DB logging
     */
    public function __invoke(array $config)
    {
        return new Logger('Database', [
            new DatabaseLoggerHandler(),
        ]);
    }
}

class DatabaseLoggerHandler extends AbstractProcessingHandler
{
    /**
     * Write the Log
     */
    protected function write(/* LogRecord */$record): void
    {
        try {
            // Convert any kind of log-record into an array
            $record = is_array($record) ? $record : $record->toArray();

            $exceptType = strtolower($record['level_name']);
            $stackTrace = ($record['context']['line'] ?? '') . "-" . ($record['context']['file'] ?? '');

            // Insert log into the database
            DB::table('logs')->insert([
                'log_level' => $exceptType,
                'timestamp' => now(),
                'exception_type' => $record['context']['type'] ?? $exceptType,
                'message' => $record['message'] ?? null,
                'stack_trace' => ($stackTrace != '-' ? $stackTrace : null),
                'source' => 'repRec-api',
                // WHY PHP? WHY?!
                // This throws an error:    'user_id' => ($record['context']['user'] ?? $record['context']['userId']) ??  null,
                // This does not:           'user_id' => $record['context']['user'] ?? $record['context']['userId'] ??  null,
                'user_id' => $record['context']['user'] ?? $record['context']['userId'] ??  null,
            ]);
        } catch (\Exception $e) {
            // In case DB logging fails, try logging to file
            Log::channel('single')->emergency("DB logging failed");
            // Log::channel('single')->error($record);
        }
    }
}
