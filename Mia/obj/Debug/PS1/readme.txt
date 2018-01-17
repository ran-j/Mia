pSX readme
-=-=-=-=-=

Introduction
============

pSX emulates the Sony Playstation 1, pretty much everything is emulated
(to my knowledge) and most games run perfectly.  It runs under Windows and
Linux.

One thing that should be noted is that pSX DOES NOT use plugins.
The emulator is completely self contained.

The emulator has been designed to be as easy and unobtrusive to use
as possible - in most cases you will not need to configure anything
to use it (except maybe the controls).

Installation
============

Extract all files from the .rar (or .tar.bz2 file under Linux) including
folders.

The emulator requires a PS1 BIOS file which should be placed in the
bios directory.  By default the emulator will look for scph1001.bin
- this version is highly recommend because it is the only well tested
one although other bioses should work.

Once the emulator is running you can change the BIOS from the
configuration menu.  In the event that you only have a different bios
file you can edit psx.ini to get the emulator running (run psx.exe
once, you will get an error message, now look for psx.ini).

Installation notes for Linux
============================

Under Linux pSX requires the following shared libraries/packages :

	OpenGL
	ALSA
	GTK
	GTKGLEXT
	libxml2

pSX will store its settings in ~/.pSX (the memcards, saves and screenshots
dirctories will also be created here).  If psx.ini is present in the
application directory it will be read from there instead (for backwards
compatibility with verions before v1.13 - you can also move psx.ini here if
you don't want per-user settings, remember to make it world writable though
otherwise users will be unable to save any settings).

Running games
=============

The emulator supports .cue/.bin, .ccd/.img/.sub, .iso, and .mdf/.mds CD
images, the easiest way to run a game is to run the exe then select "Insert CD
image" from the File menu. It is also possible to start a game from the
command line:

	psx.exe c:\psxgames\mygame.bin

or under Linux :

	pSX /usr/psxgames/mygame.bin

Note that you will get best performance running in full-screen mode.
Press ALT+ENTER to switch between full-screen and windowed.

CDZ images
==========

pSX supports compressed CD images which are called .CDZ files.  They can
either be created using the built in converted (File/Convert .BIN to/from
.CDZ) or by using the cdztool.exe command line utility.

.CDZ files can be created from all image formats that pSX supports.

To create a .cdz file utils cdztool :

	utils\cdztool.exe c:\psxgames\mygame.bin c:\psxgames\mygame.cdz

You can also convert a cdz back to .cue/.bin by reversing the order of
the parameters, eg:

	utils\cdztool.exe c:\psxgames\mygame.cdz c:\psxgames\mygame.bin

Configuration
=============

The configuration menu is fairly self explanatory.  This is a quick
run through of the various options.

Paths
-----

This tab allows you to set the default paths for state saves, memory
cards and cd images.

BIOS
----

This tab allows you to set the BIOS image used by the emulator.

Memory cards
------------

This tab configures the memory cards inserted into the Playstations
slots.  To create a memory card click ..., navigate to where you want
the file to be saved, and enter a filename (eg. "mycard").

Graphics
--------

Bilinear interpolation - enabled/disables bilinear filtering when stretching
frame buffer to window.

Frame skipping - enables/disables frame skipping

Sleep when idle in windowed mode - when running in windowed mode this
option causes the emulator to use less CPU time but can result in
choppier performance.

Pause when not focused - when enabled the emulator will pause when its
main window does not have focus (note: sound is always disabled when
not focused).

Status icons - displays status icons (CD/XA/etc..) at the bottom left

Full screen mode: can be configured separately for PAL and NTSC.  In
most cases these should be left on Default which will use the most
appropriate mode for the PS1 (Note: setting a mode that does not have
a refresh rate of 60hz for NTSC, or 100hz for PAL will result in less
smooth emulation).  You can select which monitor fullscreen mode will use
(default = use whichever monitor the window is on).  Aspect ratio correction
can also be forced in this dialog (the default setting is Auto which will
apply aspect ratio correction based on the resolution of your desktop - this
should do the correct thing for 99% of people).

Display adjustments: gamma, brightness and contrast can be configured.  These
options only affect full screen mode (most cards only support a gamma ramp in
full screen mode).

Controllers
-----------

This tab allows you to configure joystick and keyboard mappings for
the Playstations controllers.

Note that you MUST configure the gamepad controls if you want to use
a joystick or gamepad (by default only keys are set up).  Also note
that you must configure the analog axes if you want to use an analog
controller (eg. dualshock).

Default controls (for port1) :

  Up        Cursor up
  Left      Cursor left
  Down      Cursor down
  Right     Cursor right
  Triangle  A
  Square    X
  Cross     Z
  Circle    S
  Start     Return
  Select    Space
  L1        Left shift
  L2        Left ctrl
  R1        Right shift
  R2        Right ctrl

Sound
-----

From this tab you can configure sound settings.

Device: set the device used for sound output, in most cases this should
be set to "Primary Sound Driver"

Frequency: normally you should leave "Same as emulated machine" checked
which will cause the emulator to use the same frequency as the PS1.  To
change the output frequency untick the option and select from the combo
box.

Latency: This option controls how much delay there is in the sound output
in most cases the default setting will be fine, but if you experience
dropouts or choppy sound you can try increasing this.

XA latency: Controls how much delay there is in XA sound output.

Reverb: Enables/disables reverb emulation.

Sync sound: When enabled the emulator will try to keep the sound in sync
with the graphics (this should normally be enabled).

Interpolate: Enables/disables linear interpolation of sound output -
takes slightly more CPU power (not much) but reduces aliasing significantly.

Misc
----

Allows redefinition of various miscellaneous keyboard controls.


CDROM
-----

Allows selection of CDROM driver (Auto detect, IoControl or ASPI) - normally
Auto detect should be used.

ASPI Drive letter mapping under Win98/ME
========================================

Under Windows98/ME there is no way to correctly map drive letters to ASPI
drives.  Because of this the emulator makes a guess (it guesses that the
drives are in order of scsi adapter/target) - if you have multiple drives it
may guess wrong.  For this reason it is possible to override the mapping.

To see what pSX thinks the mappings are run the emulator with the -x option:

psxfin -x

This will show a list of drives with the corresponding drive letter on the
left.  If this is incorrect you can add entries to the [CDROM] section in the
.ini file to override it, for example :

[CDROM]
Driver=-1
SCSI4:1=x:

This tells the emulator that SCSI target 4:1 is drive x:

Crash dumps (Windows only)
==========================

If the emulator crashes it will ask if you want to save a crash dump.
There are two types of crash dump, mini dump and full dump - a dialog
box will ask you if you want to save a full dump.  In most cases you
will want to say no to this because full dumps can be very large - a
mini dump is usually enough to debug problems.

Dump files can be included in a bug report (see email address below).

NOTE: crash dump saving is only supported if you have dgbhelp.dll installed

History
=======

v1.0 Initial release

v1.1 ALT+F4 now exits the emulatoer
     Pressing F1/Shift+F1 switches back to windowed mode for file dialog
     *.mcr files are shown in memory card dialog
     Option to disable status icons (CD/XA/MR, etc..) - disabled by default
     dbghelp.dll is now optional (required for saving crash dumps though)
     CD drives (IoControl and ASPI) should now work
     pSX now works under Win98/ME (hopefully ;)
     Fixed save state loading bug
     Fixed crash when entering non existant save state filename

v1.2 Improved ASPI and IoControl latency
     Added SPTI support
     Added support for 16bit rendering (including 565), only used if necessary
     Full screen mode configuration (NTSC and PAL configured separately)
     Fixed GTE bug that caused Valkyrie Profile to crash
     Improved streamed ADPCM audio (Valkyrie Profile and others)

v1.3 Fixed several GTE bugs (Crash Bandicoot 3 + others)
     More accurate SPU noise and FM (FF7 and ChronoCross sound correct)
     Set mask bit for lines (MGS frequency display now works)
     Fixed bug where primitive coordinates were not sign extended (ChronoCross)
     Swapped analog sticks
     Fixed pad force feedback crash (now properly enumerates actuators)
     Disabled vsync in windowed mode to fix speed problems
     Fixed SPU looping bug: -S option is no longer required (now ignored)
     Fixed bug where starting in fullscreen would incorrectly resize window
     Fixed debugger GPU capture
     Added preliminary version of TheCloudOfSmoke's icon

v1.4 Added final icon and controller dialog graphcs from TheCloudOfSmoke
     Fixed bug where joystick device selection was not kept after restarting
     Implemented PPF patch support (command line only for now)
     Added option to disable bilinear filtering
     Added L3 and R3 buttons for dualshock controller
     Added reverse axis option for controller analog axes
     Option to disable controller rumble now works
     Second controller now works
     Fixed keys that did not work in controller config dialog (eg. cursors)
     Integrated CDZ converter into main program
     Added gamma, brightness and contrast controls (fullscreen only)
     Added quicksave/load functionality
     Fast forward now temporarily turns off vsync
     ASPI DLL is no longer required if using IOControl
     Fixed state save dialog (now uses save type instead of open)
     Fixed window painting when paused (or in modal dialogs)
     Removed log menu option in release version
     Editing code in debugger memory window works even when using recompiler
     Fixed some other debugger problems
     Implemented breakpoint editing
     Fixed save state loading bug

v1.5 Added screenshot feature
     Fixed GTE bug that caused graphical glitches in Legend of Legia
     Fixed minor CDROM emulation bug
     Adjusted GPU DMA timing to fix MDEC hangs in some games (Legend of Legia)
     Check mask bit for lines (fixes DragonWarrior 7 menus)
     Fixed spelling of "Quicksave" (yes, I suppose I am a "d u m b a s s" ;)
     Use exe name for quicksaves (quicksaves are now per game)
     Changed key assignments for quicksave (F1-F5=load, F6-F10=save)
     Reset button for gamma/brightness/contrast in graphics dialog now works
     ESC now quits emulator (in addition to ALT+F4)
     Added DMA breakpoints
     Fixed some miscellaneous debugger bugs

v1.6 Fixed missing characters in WildArms battles (GTE flags inaccuracy)
     Fixed GTE bugs that caused missing graphics in Tombraider Chronicles
     Fixed recompiler bug that cause Tomraider Chronicles to crash
     Add missing CDROM functionality that caused DW7 to hang during FMVs
     Emulator can now be full screened on any monitor (autodetect or force)
     Fixed bug that caused XA audio to cut out (often during fastforward)
     Added support for CloneCD and ISO CD images
     Made BIN images work even if CUE file is missing (not recommended)
     Added support for new CD image types to CDZ
     Corrected aspect ratio in PAL modes
     Fixed bug where quick saves didn't work with certain games
     Fixed bug where F10 entered the menu instead of loading quick save
     More accurate SPU ADSR envelopes (including exponential modes)
     Initialise ADSR registers after SPU reset (fixes sound in Tombraider)

v1.7 Subcode reading (libcrypt games should now work)
     Better support for self-modifying code (fixes Spyro3)
     More anti-mod protected games now work (tested on WildArms2)
     Added support for MDEC STP bit (fixes DragonWarrior7 spell effects)
     Fixed quickload menu bug
     Fixed crash loading v1.5 state saves
     Fixed bug where .bin file was named .cue when converting CDZ file
     Fixed bug where CDZs created without .cue file did not work
     ESC can now be configured (either quit, or exit fullscreen)
     CD drivers now retry when an error occurs (up to 16 times)
     Fixed bug where switching back to windowed sometimes didn't work
     File requester is now displayed when BIOS is not found

v1.8 Various GTE fixes (fixes Wipeout, Tony Hawk Pro Skater 2, amongst others)
     Fixed bug where self-modifying code was incorrectly detected
     Save/restore debugger window layout
     Debugger font can now be configured
     Fixed crash when memory card file did not exist but was referred to by ini
     Implemented CCD parsing
     Improved CUE file handling (pre/post gaps and indexes now supported)
     Fixed CD play command and report mode (fixes BIOS cd player)
     Ignore non mode1 sub-q sectors (required when playing audio CDs)
     Corrected sub-q faking when subcode reading is not enabled
     Fixed bug where CDDA playing would stop when fast forwarding
     Fixed crash when ejecting CD while game is reading it
     Fixed crash when frame buffer is bigger than display mode (VibRibbon PAL)
     All combo boxes in config menu are now read-only
     Improved emulation of SPU CD and reverb buffer (VibRibbon)
     Support SPU IRQs in CD buffer (VibRibbon)
     Fixed crash after recovering from Ctrl-Alt-Del
     Fixed bug where gamma settings did not update until pressing adjust key

v1.9 Various CDROM changes to make Ape Escape work
     Removed incorrect ADPCM autopause functionality (fixes G-Police music)
     More accurate emulation of GPU LCF status bit (fixes G-Police hang)
     Implemented sprite flip draw mode bits (Master system emulator)
     Various root counter emulation improvements (fixes Rhapsody)
     Slight timing change to fix problem where pad stops working intermittantly
     Fixed bug where Crash Bandicoot 2 executes illegal opcodes after intro
     Fixed WildArms XA spell effect bug (didn't stop after effect correctly)
     Changes to CDROM emulation to make Ape Escape work
     Reject large polygons (fixes ChronoCross Dragonia bug and SagaFrontier2)

v1.10 Allow ADSR changes while voice keyed on (fixes sounds in FF7 and others)
      Localisation for various languages
      Added internal manifest file (dialogs now support XP theme correctly)
      Sound device can now be set to disabled
      Fixed bug where sound was muted when reverb was disabled
      Fixed Chrono Cross hangs introduced in v1.9
      Breakpoints can now be added/removed in debugger while CPU is running
      Added check for valid BIOS

v1.11 Ported to Linux
      Log window keyboard controls
      Added Arabic, Croation, Norwegian, Persian and Russian translations
      Fixed bug in CD not usable error message dialog
      DEP no longer needs to be disabled in Windows for pSX to work
      Added aspect ratio correction
      Centred framebuffer when it doesn't cover the entire screen
      Fixed controller config dialog bug (pressing button fills all controls)

v1.12 Fixed crash on startup with -f command line option
      Fixed SPU bug that caused FF8 FMV audio to stop sometimes
      Fixed bug that prevented some keys being mapped to controllers
      Fixed bug where window size/position was reset when using fast forward
      Added support for Alcohol 120% MDF/MDS images
      Fake subcode in track gaps (required for TombRaider1)
      Fixed Syphon Filter boot hang
      Fixed infinite loop in Tekken3 and Deception3
      Changed 384 mode to 364 which seems to be correct (based on TombRaider)
      Fixed aspect ratio correction for 5:4

v1.13 Added Korean, Bosian, Serbian and Icelandic translations
      Added some missing translations to Linux build
      Debugger DMA capture buffer now autoresizes
      Fixed streaming music in Jikkyou Oshaberi Parodius
      Fixed bug when setting sound frequency in Windows
      Implemented volume and mute in Linux
      Fixed "missing body parts" bug in Deception 3
      Fixed bug that caused XA audio in Deception 3 to not stop correctly
      Fixed random crash in Road Rash: Jailbreak
      Fixed debugger crash when emulator is reading from CD
      Fixed hang opening CD images in Linux
      Per-user settings for Linux (.ini file is now stored in ~/.pSX)
      Removed SSE instructions used during init (should fix crash on AMD CPUs)
      Fixed GTK warnings when clicking window close button in Linux

Credits
=======

Icon and controller dialog graphics: TheCloudOfSmoke
Polish translation: Wojciech "R4Zi3L" Olczyk
Danish translation: Sune Mika Salminen
Dutch translation: Talisman (with proofreading by Patrick van Arkel)
Spanish translation: anewuser
Finnish translation: Mika Heiska
Portuguese (Brazil) translation: Melanogaster
German translation: BlackVivi
Italian translation: Soulbrighter
Simplified Chinese translation: Monica
Traditional Chinese translation: nhlay
Japanese translation: Nekokabu
French translation: Ryusan
Swedish translation: Kaputnik
Croatian translation: Shendo
Russian translation: d0tter
Norwegian translation: Roman Alifanov
Arabic translation: Nzaar9
Bosnian and Serbian translations: Cobalt
Korean translation: Park9119
Icelandic translation: Grendel Hilmarsson
Everything else: pSX Author ;)

Contact
=======

Home page: http://psxemulator.gazaxian.com
Forum: http://psxemulator.proboards54.com
Questions or bug reports should be directed to: psxemulator@googlemail.com
