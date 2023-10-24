import { forwardRef } from 'react';

import Box from '@mui/joy/Box';
import ButtonGroup, { type ButtonGroupProps } from '@mui/joy/ButtonGroup';
import IconButton from '@mui/joy/IconButton';

import {
  BoldIcon,
  ItalicIcon,
  UnderlineIcon,
  StrikethroughIcon,
  QuoteIcon,
  SubscriptIcon,
  SuperscriptIcon,
  OutdentIcon,
  IndentIcon,
  ListOrderedIcon,
  ListIcon,
  Heading2Icon,
  Heading3Icon,
  Heading4Icon,
  AlignLeftIcon,
  AlignCenterIcon,
  AlignRightIcon,
  AlignJustifyIcon,
  Link2Icon,
  ImageIcon,
  VideoIcon,
  Code2Icon,
  RemoveFormattingIcon,
  Heading6Icon,
  Heading5Icon,
  CaseSensitiveIcon,
  Heading1Icon,
} from 'lucide-react';

const Toolbar = forwardRef<HTMLDivElement>((props: Omit<ButtonGroupProps, 'ref'>, ref) => {
  const { sx, ...rest } = props;

  return (
    <ButtonGroup
      className="editor-toolbar"
      size="sm"
      variant="plain"
      sx={[
        {
          flexWrap: 'wrap',
          gap: 0.5,

          '& > button': {
            borderRadius: '4px !important',
          },
        },
        ...(Array.isArray(sx) ? sx : [sx]),
      ]}
      {...rest}
      ref={ref}
    >
      <IconButton className="ql-bold">
        <BoldIcon />
      </IconButton>
      <IconButton className="ql-italic">
        <ItalicIcon />
      </IconButton>
      <IconButton className="ql-underline">
        <UnderlineIcon />
      </IconButton>
      <IconButton className="ql-strike">
        <StrikethroughIcon />
      </IconButton>
      <IconButton className="ql-blockquote">
        <QuoteIcon />
      </IconButton>
      <Box className="editor-separator" />
      <IconButton className="ql-script" value="sub">
        <SubscriptIcon />
      </IconButton>
      <IconButton className="ql-script" value="super">
        <SuperscriptIcon />
      </IconButton>
      <Box className="editor-separator" />
      <IconButton className="ql-indent" value="-1">
        <OutdentIcon />
      </IconButton>
      <IconButton className="ql-indent" value="+1">
        <IndentIcon />
      </IconButton>
      <Box className="editor-separator" />
      <IconButton className="ql-list" value="ordered">
        <ListOrderedIcon />
      </IconButton>
      <IconButton className="ql-list" value="bullet">
        <ListIcon />
      </IconButton>
      <Box className="editor-separator" />
      <IconButton className="ql-header" value="0">
        <CaseSensitiveIcon />
      </IconButton>
      <IconButton className="ql-header" value="1">
        <Heading1Icon />
      </IconButton>
      <IconButton className="ql-header" value="2">
        <Heading2Icon />
      </IconButton>
      <IconButton className="ql-header" value="3">
        <Heading3Icon />
      </IconButton>
      <IconButton className="ql-header" value="4">
        <Heading4Icon />
      </IconButton>
      <IconButton className="ql-header" value="5">
        <Heading5Icon />
      </IconButton>
      <IconButton className="ql-header" value="6">
        <Heading6Icon />
      </IconButton>
      <Box className="editor-separator" />
      <IconButton className="ql-align" value="left">
        <AlignLeftIcon />
      </IconButton>
      <IconButton className="ql-align" value="center">
        <AlignCenterIcon />
      </IconButton>
      <IconButton className="ql-align" value="right">
        <AlignRightIcon />
      </IconButton>
      <IconButton className="ql-align" value="justify">
        <AlignJustifyIcon />
      </IconButton>
      <Box className="editor-separator" />
      <IconButton className="ql-link">
        <Link2Icon />
      </IconButton>
      <IconButton className="ql-image">
        <ImageIcon />
      </IconButton>
      <IconButton className="ql-video">
        <VideoIcon />
      </IconButton>
      <Box className="editor-separator" />
      <IconButton className="ql-code-block">
        <Code2Icon />
      </IconButton>
      <IconButton className="ql-clean">
        <RemoveFormattingIcon />
      </IconButton>
    </ButtonGroup>
  );
});

Toolbar.displayName = 'Toolbar';

export default Toolbar;
