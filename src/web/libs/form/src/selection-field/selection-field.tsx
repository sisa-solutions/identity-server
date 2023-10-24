import { type Control, Controller, type FieldPath, type FieldValues } from 'react-hook-form';

import SelectInput, { type SelectionInputProps, type SelectOption } from '../selection-input';

type SelectionFieldProps<
  T extends string | number,
  O extends SelectOption<T>,
  TFieldValues extends FieldValues = FieldValues,
  TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
> = {
  name: TName;
  control: Control<TFieldValues>;
} & SelectionInputProps<T, O>;

const SelectionField = <
  T extends string | number,
  O extends SelectOption<T>,
  TFieldValues extends FieldValues = FieldValues,
>({
  name,
  control,
  ...rest
}: SelectionFieldProps<T, O, TFieldValues>) => {
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

export default SelectionField;
