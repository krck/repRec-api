<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration {
    public function up()
    {
        Schema::create('users', function (Blueprint $table) {
            $table->string('id')->primary(); // Auth0 user_id as the primary key (non-incrementing)
            $table->string('email')->unique();
            $table->boolean('email_verified')->default(false);
            $table->string('nickname')->nullable();
            $table->string('setting_timezone');
            $table->string('setting_weight_unit');
            $table->string('setting_distance_unit');
            $table->timestamps(); // Adds created_at and updated_at
        });
    }

    public function down()
    {
        Schema::dropIfExists('users');
    }
};
