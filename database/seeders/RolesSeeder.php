<?php

namespace Database\Seeders;

use App\Common\Enums\EnumRoles;
use Illuminate\Database\Seeder;
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
