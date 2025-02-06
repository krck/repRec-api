<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Roles extends Model
{
    /** @use HasFactory<\Database\Factories\RolesFactory> */
    use HasFactory;

    protected $table = 'roles'; // Name of the table

    protected $primaryKey = 'id'; // Primary key
    protected $fillable = ['name']; // Mass-assignable fields
    public $timestamps = false; // No created_at and updated_at columns

}
