<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    /**
     * Run the migrations.
     */
    public function up(): void
    {
        Schema::create('LogLevels', function (Blueprint $table) {
            $table->integer('Id')->primary();
            $table->text('Name');
        });

        Schema::create('Logs', function (Blueprint $table) {
            $table->integer('Id')->primary();
            $table->integer('LogLevelId')->index('ix_logs_loglevelid');
            $table->timestampTz('Timestamp');
            $table->text('ExceptionType');
            $table->text('Message')->nullable();
            $table->text('StackTrace')->nullable();
            $table->text('Source')->nullable();
            $table->text('userId')->nullable();
        });

        Schema::create('OptExerciseCategories', function (Blueprint $table) {
            $table->integer('Id')->primary();
            $table->text('Name');
            $table->text('JsonTemplate');
            $table->text('Description')->nullable();
        });

        Schema::create('OptExercises', function (Blueprint $table) {
            $table->integer('Id')->primary();
            $table->text('Name');
            $table->text('Mechanic')->nullable();
            $table->text('PrimaryMuscles');
            $table->text('SecondaryMuscles')->nullable();
            $table->text('Force')->nullable();
            $table->text('Level')->default('');
            $table->text('Equipment')->nullable();
            $table->text('Instructions')->nullable();
            $table->integer('OptExerciseCategoryId')->default(0)->index('ix_optexercises_optexercisecategoryid');
        });

        Schema::create('PlanWorkoutExercises', function (Blueprint $table) {
            $table->integer('Id')->primary();
            $table->integer('PlanWorkoutId')->index('ix_planworkoutexercises_planworkoutid');
            $table->integer('OptExerciseCategoryId')->index('ix_planworkoutexercises_optexercisecategoryid');
            $table->integer('OptExerciseId')->index('ix_planworkoutexercises_optexerciseid');
            $table->integer('DayIndex');
            $table->integer('DayOrder');
            $table->text('ExerciseDefinitionJson');
            $table->text('UserId')->default('')->index('ix_planworkoutexercises_userid');
        });

        Schema::create('PlanWorkouts', function (Blueprint $table) {
            $table->integer('Id')->primary();
            $table->text('Name');
            $table->text('Description')->nullable();
            $table->text('UserId')->default('')->index('ix_planworkouts_userid');
            $table->timestampTz('CreatedAt')->default('-infinity');
            $table->boolean('IsShared')->default(false);
        });

        Schema::create('Roles', function (Blueprint $table) {
            $table->integer('Id')->primary();
            $table->text('Name');
        });

        Schema::create('UserRoles', function (Blueprint $table) {
            $table->text('UserId');
            $table->integer('RoleId')->index('ix_userroles_roleid');

            $table->primary(['UserId', 'RoleId']);
        });

        Schema::create('Users', function (Blueprint $table) {
            $table->text('Id')->primary();
            $table->text('Email');
            $table->boolean('EmailVerified');
            $table->text('Nickname')->nullable();
            $table->timestampTz('CreatedAt');
            $table->text('SettingDistanceUnit')->default('');
            $table->text('SettingTimezone')->default('');
            $table->text('SettingWeightUnit')->default('');
        });

        Schema::create('__EFMigrationsHistory', function (Blueprint $table) {
            $table->string('MigrationId', 150)->primary();
            $table->string('ProductVersion', 32);
        });

        Schema::table('Logs', function (Blueprint $table) {
            $table->foreign(['LogLevelId'], 'FK_Logs_LogLevels_LogLevelId')->references(['Id'])->on('LogLevels')->onUpdate('no action')->onDelete('cascade');
        });

        Schema::table('OptExercises', function (Blueprint $table) {
            $table->foreign(['OptExerciseCategoryId'], 'FK_OptExercises_OptExerciseCategories_OptExerciseCategoryId')->references(['Id'])->on('OptExerciseCategories')->onUpdate('no action')->onDelete('cascade');
        });

        Schema::table('PlanWorkoutExercises', function (Blueprint $table) {
            $table->foreign(['OptExerciseCategoryId'], 'FK_PlanWorkoutExercises_OptExerciseCategories_OptExerciseCateg~')->references(['Id'])->on('OptExerciseCategories')->onUpdate('no action')->onDelete('cascade');
            $table->foreign(['OptExerciseId'], 'FK_PlanWorkoutExercises_OptExercises_OptExerciseId')->references(['Id'])->on('OptExercises')->onUpdate('no action')->onDelete('cascade');
            $table->foreign(['PlanWorkoutId'], 'FK_PlanWorkoutExercises_PlanWorkouts_PlanWorkoutId')->references(['Id'])->on('PlanWorkouts')->onUpdate('no action')->onDelete('cascade');
            $table->foreign(['UserId'], 'FK_PlanWorkoutExercises_Users_UserId')->references(['Id'])->on('Users')->onUpdate('no action')->onDelete('cascade');
        });

        Schema::table('PlanWorkouts', function (Blueprint $table) {
            $table->foreign(['UserId'], 'FK_PlanWorkouts_Users_UserId')->references(['Id'])->on('Users')->onUpdate('no action')->onDelete('cascade');
        });

        Schema::table('UserRoles', function (Blueprint $table) {
            $table->foreign(['RoleId'], 'FK_UserRoles_Roles_RoleId')->references(['Id'])->on('Roles')->onUpdate('no action')->onDelete('cascade');
            $table->foreign(['UserId'], 'FK_UserRoles_Users_UserId')->references(['Id'])->on('Users')->onUpdate('no action')->onDelete('cascade');
        });
    }

    /**
     * Reverse the migrations.
     */
    public function down(): void
    {
        Schema::table('UserRoles', function (Blueprint $table) {
            $table->dropForeign('FK_UserRoles_Roles_RoleId');
            $table->dropForeign('FK_UserRoles_Users_UserId');
        });

        Schema::table('PlanWorkouts', function (Blueprint $table) {
            $table->dropForeign('FK_PlanWorkouts_Users_UserId');
        });

        Schema::table('PlanWorkoutExercises', function (Blueprint $table) {
            $table->dropForeign('FK_PlanWorkoutExercises_OptExerciseCategories_OptExerciseCateg~');
            $table->dropForeign('FK_PlanWorkoutExercises_OptExercises_OptExerciseId');
            $table->dropForeign('FK_PlanWorkoutExercises_PlanWorkouts_PlanWorkoutId');
            $table->dropForeign('FK_PlanWorkoutExercises_Users_UserId');
        });

        Schema::table('OptExercises', function (Blueprint $table) {
            $table->dropForeign('FK_OptExercises_OptExerciseCategories_OptExerciseCategoryId');
        });

        Schema::table('Logs', function (Blueprint $table) {
            $table->dropForeign('FK_Logs_LogLevels_LogLevelId');
        });

        Schema::dropIfExists('__EFMigrationsHistory');

        Schema::dropIfExists('Users');

        Schema::dropIfExists('UserRoles');

        Schema::dropIfExists('Roles');

        Schema::dropIfExists('PlanWorkouts');

        Schema::dropIfExists('PlanWorkoutExercises');

        Schema::dropIfExists('OptExercises');

        Schema::dropIfExists('OptExerciseCategories');

        Schema::dropIfExists('Logs');

        Schema::dropIfExists('LogLevels');
    }
};
