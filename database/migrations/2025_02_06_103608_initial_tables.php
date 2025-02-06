<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    /**
     * Run the migrations.
     */
    public function up(): void
    {
        Schema::create('logs', function (Blueprint $table) {
            $table->increments('id')->primary();
            $table->text('log_level');
            $table->timestampTz('timestamp');
            $table->text('exception_type');
            $table->text('message')->nullable();
            $table->text('stack_trace')->nullable();
            $table->text('source')->nullable();
            $table->text('user_id')->nullable();
        });

        Schema::create('roles', function (Blueprint $table) {
            $table->integer('id')->primary();
            $table->text('name');
        });
    }

    /**
     * Reverse the migrations.
     */
    public function down(): void
    {
        Schema::dropIfExists('roles');

        Schema::dropIfExists('logs');
    }
};
