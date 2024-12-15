# Contribution guide to the plugin

## The data structures

In order to make manipulating the data easier across the possible buttons, the following structures exist in `ClipboardBuddyDataStruct.cs`:

- (GENERIC) UsefulColors
- (COORD) ColorUsedMatrix
- (COORD) TextStorageMatrix
- (COORD) ConnectionMatrix
- (COORD) InitialImageNameMatrix

As you can see, most of the entries are indexed by key coordinates to easily pick items across the hashtables and yet be able to have an overview of which keys are active.
