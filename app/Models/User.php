<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class User extends Model
{
    /** @use HasFactory<\Database\Factories\UserFactory> */
    use HasFactory;

    protected $table = 'users';

    // Define primary key as a not incrementing string
    // (PK is the Auth0 user_id)
    protected $primaryKey = 'id';
    public $incrementing = false;
    protected $keyType = 'string';

    public $timestamps = true;

    // Assignable / Mass-Assignable fields
    protected $fillable = [
        'id',
        'email', // Unique constraint
        'email_verified',
        'nickname',
        'setting_timezone',
        'setting_weight_unit',
        'setting_distance_unit',
        'created_at',
        'updated_at'
    ];

    public function roles()
    {
        return $this->belongsToMany(Role::class, 'user_roles');
    }
}
