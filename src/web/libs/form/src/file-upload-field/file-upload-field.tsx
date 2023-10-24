import { Control, Controller, type FieldPath, type FieldValues } from 'react-hook-form';

import FileUploadInput, { type FileUploadInputProps } from '../file-upload-input';

type FileUploadFieldProps<
  TFieldValues extends FieldValues = FieldValues,
  TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
> = {
  name: TName;
  control: Control<TFieldValues>;
} & FileUploadInputProps;

const FileUploadField = <TFieldValues extends FieldValues>({
  name,
  control,
  ...rest
}: FileUploadFieldProps<TFieldValues>) => {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field: { onChange, ...restFieldProps }, fieldState: { invalid, error } }) => {
        return (
          <FileUploadInput
            {...rest}
            {...restFieldProps}
            onChange={(files) => {
              onChange(files);
            }}
            error={!!error || invalid}
            errorMessage={error?.message}
          />
        );
      }}
    />
  );
};

export default FileUploadField;
