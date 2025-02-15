<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Role extends Model
{
    /** @use HasFactory<\Database\Factories\RoleFactory> */
    use HasFactory;

    protected $table = 'roles'; // Name of the table

    protected $primaryKey = 'id'; // Primary key
    protected $fillable = ['name']; // Mass-assignable fields
    public $timestamps = false; // No created_at and updated_at columns

}
