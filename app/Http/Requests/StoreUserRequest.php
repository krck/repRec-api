<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class StoreUserRequest extends FormRequest
{
    /**
     * Determine if the user is authorized to make this request.
     */
    public function authorize(): bool
    {
        return false;
    }

    /**
     * Get the validation rules that apply to the request.
     *
     * @return array<string, \Illuminate\Contracts\Validation\ValidationRule|array<mixed>|string>
     */
    public function rules(): array
    {
        return [
            'id' => 'required|string|unique:users,id',
            'email' => 'required|email|unique:users,email',
            'email_verified' => 'boolean',
            'nickname' => 'nullable|string',
            'setting_timezone' => 'required|string',
            'setting_weight_unit' => 'required|string',
            'setting_distance_unit' => 'required|string',
        ];
    }
}
