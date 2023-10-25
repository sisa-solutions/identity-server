import { type Control, Controller, type FieldPath, type FieldValues } from 'react-hook-form';

import PasswordInput, { type PasswordInputProps } from '../password-input';

type PasswordFieldProps<
  TFieldValues extends FieldValues = FieldValues,
  TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
> = {
  name: TName;
  control: Control<TFieldValues>;
} & PasswordInputProps;

const PasswordField = <TFieldValues extends FieldValues>({
  name,
  control,
  ...rest
}: PasswordFieldProps<TFieldValues>) => {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field, fieldState: { invalid, error } }) => {
        return (
          <PasswordInput
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

export default PasswordField;
