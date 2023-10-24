"use client";

import 'quill/dist/quill.core.css';

import { useEffect, useRef, useState, forwardRef } from 'react';

import Textarea, { type TextareaProps } from '@mui/joy/Textarea';

import Toolbar from './toolbar';
import EditorContent from './editor-content';

export type RichTextProps = Omit<TextareaProps, 'value' | 'defaultValue' | 'onChange'> & {
  value?: string;
  defaultValue?: string;
  onChange?: (value: string) => void;
};

const RichText = forwardRef<HTMLDivElement, RichTextProps>(
  ({ value, defaultValue, readOnly, onChange, ...rest }, ref) => {
    const [isMounted, setIsMounted] = useState(false);
    const [richTextValue, setRichTextValue] = useState(value ?? defaultValue ?? '');

    const editorRef = useRef<HTMLDivElement | null>(null);
    const toolbarRef = useRef<HTMLDivElement | null>(null);

    useEffect(() => {
      setIsMounted(true);
    }, []);

    useEffect(() => {
      (async () => {
        if (!isMounted || !editorRef.current || !toolbarRef.current) return;

        const Quill = (await import('quill')).default;

        const icons = Quill.import('ui/icons');

        Object.keys(icons).forEach((key) => {
          icons[key] = null;
        });

        /*
         * Quill modules to attach to editor
         * See https://quilljs.com/docs/modules/ for complete options
         */
        const quill = new Quill(editorRef.current, {
          // theme: 'snow',
          readOnly: readOnly,
          modules: {
            toolbar: toolbarRef.current,
            clipboard: {
              // toggle to add extra line breaks when pasting HTML:
              matchVisual: false,
            },
          },
        });

        quill.on('text-change', () => {
          handleValueChange(quill.root.innerHTML);
        });

        quill.root.innerHTML = richTextValue;
      })();
      // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [isMounted]);

    const handleValueChange = (value: string) => {
      setRichTextValue(value);

      if (onChange) {
        onChange(value);
      }
    };

    if (!isMounted) return null;

    return (
      <Textarea
        ref={ref}
        className="editor-content"
        slots={{
          textarea: EditorContent,
        }}
        slotProps={{
          textarea: {
            ref: editorRef as any,
            sx: {
              '& .ql-editor': {
                p: 0,
                minHeight: (theme) => theme.spacing(8),
              },
            },
          },
        }}
        sx={{
          '& .editor-toolbar': {
            '& > .editor-separator': {
              mx: 1,
              border: 'none !important',
            },
            '& > button': {
              '--IconButton-size': '1.875rem',
              border: 'none !important',

              '& > svg': {
                color: (theme) => theme.palette.text.secondary,
              },
            },

            '& .ql-active': {
              backgroundColor: (theme) => theme.palette.primary.softBg,

              '& > svg': {
                stroke: (theme) => theme.palette.primary.softColor,
              },
            },
          },
        }}
        startDecorator={<Toolbar ref={toolbarRef} />}
        readOnly={readOnly}
        {...rest}
      />
    );
  }
);

RichText.displayName = 'RichText';

export default RichText;
