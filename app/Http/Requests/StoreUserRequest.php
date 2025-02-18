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
        return true;
    }

    /**
     * Get the validation rules that apply to the POST/CREATE request
     */
    public function rules(): array
    {
        return [
            'id' => 'required|string', // dont check "unique" id here
            'email' => 'required|email', // dont check "unique" email here
            'email_verified' => 'boolean',
            'nickname' => 'nullable|string',
            'setting_timezone' => 'required|string',
            'setting_weight_unit' => 'required|string',
            'setting_distance_unit' => 'required|string',
        ];
    }
}
