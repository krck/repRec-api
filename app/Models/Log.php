<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Log extends Model
{
    /** @use HasFactory<\Database\Factories\LogFactory> */
    use HasFactory;

    protected $table = 'logs'; // Name of the table

    protected $primaryKey = 'id'; // Primary key
    protected $fillable = [
        'log_level',
        'timestamp',
        'exception_type',
        'message',
        'stack_trace',
        'source',
        'user_id'
    ];
    public $timestamps = false; // Disable default timestamps
}
