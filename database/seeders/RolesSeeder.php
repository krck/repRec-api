<?php

namespace Database\Seeders;

use Illuminate\Database\Console\Seeds\WithoutModelEvents;
use Illuminate\Database\Seeder;
use App\Enums\EnumRoles;
use App\Models\Roles;

class RolesSeeder extends Seeder
{
    public function run(): void
    {
        // Seed Roles with Enum values
        foreach (EnumRoles::cases() as $role) {
            Roles::updateOrCreate(
                ['id' => $role->value],
                ['name' => $role->name]
            );
        }
    }
}
