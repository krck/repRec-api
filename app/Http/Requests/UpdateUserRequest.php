<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class UpdateUserRequest extends FormRequest
{
    /**
     * Determine if the user is authorized to make this request.
     */
    public function authorize(): bool
    {
        return true;
    }

    /**
     * Get the validation rules that apply to the PUT/UPDATE request
     */
    public function rules(): array
    {
        return [
            'setting_timezone' => 'string|required',
            'setting_weight_unit' => 'string|required',
            'setting_distance_unit' => 'string|required',
        ];
    }
}
