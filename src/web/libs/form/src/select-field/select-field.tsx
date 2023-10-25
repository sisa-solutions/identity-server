import { type Control, Controller, type FieldPath, type FieldValues } from 'react-hook-form';

import SelectInput, { type SelectInputProps, type SelectOption } from '../select-input';

type SelectFieldProps<
  T extends string | number,
  O extends SelectOption<T>,
  TFieldValues extends FieldValues = FieldValues,
  TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
> = {
  name: TName;
  control: Control<TFieldValues>;
} & SelectInputProps<T, O>;

const SelectField = <
  T extends string | number,
  O extends SelectOption<T>,
  TFieldValues extends FieldValues = FieldValues,
>({
  name,
  control,
  ...rest
}: SelectFieldProps<T, O, TFieldValues>) => {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field: { onChange, ...restFieldProps }, fieldState: { invalid, error } }) => {
        return (
          <SelectInput
            {...rest}
            {...restFieldProps}
            onChange={(_, newValue) => onChange(newValue)}
            error={!!error || invalid}
            errorMessage={error?.message}
          />
        );
      }}
    />
  );
};

export default SelectField;
