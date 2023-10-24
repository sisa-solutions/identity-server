'use client';

import { forwardRef, useCallback, useEffect, useState } from 'react';

import {
  useDropzone,
  type DropzoneOptions,
  type FileRejection,
  type DropEvent,
} from 'react-dropzone';

import Box from '@mui/joy/Box';
import Card from '@mui/joy/Card';
import Typography from '@mui/joy/Typography';
import Link from '@mui/joy/Link';
import Stack from '@mui/joy/Stack';

import { guid } from '@sisa/utils';

import Preview, { type FileWithPreview } from './preview';

import FormControl from '@mui/joy/FormControl';
import FormLabel from '@mui/joy/FormLabel';
import { type InputProps } from '@mui/joy/Input';
import FormHelperText from '@mui/joy/FormHelperText';

import { UploadCloudIcon } from 'lucide-react';

export type FileUploadInputProps = InputProps & {
  options: DropzoneOptions;

  label?: React.ReactNode;
  helperMessage?: React.ReactNode;
  errorMessage?: React.ReactNode;
  onChange?: (files: Array<File>) => void;
  // uploadFn: (file: File) => Promise<void>;
};

const FileUploadInput = forwardRef<HTMLDivElement, FileUploadInputProps>(
  (
    { options, label, helperMessage, errorMessage, required, error, disabled, sx, size, onChange },
    ref
  ) => {
    const [files, setFiles] = useState<Array<FileWithPreview>>([]);
    const [, setRejected] = useState<Array<FileRejection>>([]);

    useEffect(() => {
      // Make sure to revoke the data uris to avoid memory leaks, will run on unmount
      if (onChange) onChange(files);

      return () => files.forEach((file) => URL.revokeObjectURL(file.preview));
    }, [files]);

    const onDrop = useCallback(
      (acceptedFiles: Array<File>, rejectedFiles: Array<FileRejection>, event: DropEvent) => {
        if (acceptedFiles?.length) {
          setFiles((prevFiles) => {
            const newFiles = acceptedFiles.map<FileWithPreview>((file) =>
              Object.assign(file, {
                id: guid(),
                preview: URL.createObjectURL(file),
              })
            );

            if (options.multiple) {
              return [...prevFiles, ...newFiles];
            } else {
              return newFiles;
            }
          });
        }

        if (rejectedFiles?.length) {
          setRejected((prevFiles) => [...prevFiles, ...rejectedFiles]);
        }
      },
      []
    );

    const { getRootProps, getInputProps } = useDropzone({
      ...options,
      onDrop,
    });

    const handleRemove = (id: string) => {
      setFiles((prevFiles) => prevFiles.filter((file) => file.id !== id));
    };

    // const handleRemoveAll = () => {
    //   setFiles([]);
    //   setRejected([]);
    // }

    return (
      <FormControl required={required} error={error} disabled={disabled} sx={sx} size={size}>
        <Box>
          {label && <FormLabel>{label}</FormLabel>}
          {helperMessage && <Typography level="body-sm">{helperMessage}</Typography>}
        </Box>

        <Stack direction="column" gap={1}>
          <Card
            variant="outlined"
            sx={{
              boxShadow: 'none',
            }}
            ref={ref}
            {...getRootProps({ className: 'dropzone' })}
          >
            <Box
              sx={{
                gap: 2,
                display: 'flex',
                flexDirection: 'row',
                alignItems: 'center',
              }}
            >
              <Box sx={{ p: 1, bgcolor: 'primary.softBg', borderRadius: '50%' }}>
                <Box
                  className="animate-blink"
                  sx={{
                    borderRadius: '50%',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    width: 32,
                    height: 32,
                    bgcolor: 'primary.softHoverBg',

                    '& > svg': {
                      stroke: 'primary.softColor',
                    },
                  }}
                >
                  <UploadCloudIcon />
                </Box>
              </Box>

              <Typography level="body-sm">
                <Link component="button">Click to upload</Link> or drag and drop
                <br /> SVG, PNG, JPG or GIF (max. 800x400px)
              </Typography>
            </Box>
            <input {...getInputProps()} />
          </Card>

          {errorMessage && <FormHelperText>{errorMessage}</FormHelperText>}

          {files.length > 0 && (
            <Stack direction="row" spacing={{ xs: 1, sm: 2 }} useFlexGap flexWrap="wrap">
              {files.map((file) => (
                <Preview key={file.id} file={file} handleRemove={handleRemove} />
              ))}
            </Stack>
          )}
        </Stack>
      </FormControl>
    );
  }
);

FileUploadInput.displayName = 'FileUploadInput';

export default FileUploadInput;
