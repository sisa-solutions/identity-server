import { Control, Controller, type FieldPath, type FieldValues } from 'react-hook-form';

import PatternFormatInput, { type PatternFormatInputProps } from '../pattern-format-input';

type PatternFormatFieldProps<
  TFieldValues extends FieldValues = FieldValues,
  TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
> = {
  name: TName;
  control: Control<TFieldValues>;
} & PatternFormatInputProps;

const PatternFormatField = <TFieldValues extends FieldValues>({
  name,
  control,
  ...rest
}: PatternFormatFieldProps<TFieldValues>) => {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field, fieldState: { invalid, error } }) => {
        return (
          <PatternFormatInput
            {...rest}
            {...field}
            error={!!error || invalid}
            errorMessage={error?.message}
          />
        );
      }}
    />
  );
};

export default PatternFormatField;
