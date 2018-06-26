/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation files (the "Software"), to deal 
 *  in the Software without restriction, including without limitation the rights 
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 *  copies of the Software, and to permit persons to whom the Software is 
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 *  THE SOFTWARE. 
 */

using PortableDeviceApiLib;
using System;

namespace Alphaleonis.Win32.Device
{
   /// <summary>Windows Portable Device (WPD) Constants.</summary>
   /// <remarks>Add Reference to Interop.PortableDeviceApiLib and Interop.PortableDeviceTypesLib and set "Embed Interop Types" to <c>False</c>.</remarks>
   internal static partial class NativeMethods
   {
      #region Protocol ID

      /// <summary>("MSC:") The ID string of the Mass Storage Class protocol.</summary>
      internal const string MassStorageClassProtocol = "MSC:";

      /// <summary>("MTP: 1.00") The ID string of the Media Transfer Class protocol.</summary>
      internal const string MediaTransferProtocol = "MTP: 1.00";

      #endregion // Protocol ID


      #region Client Properties

      private static readonly Guid ClientGuid = new Guid(0x204D9F0C, 0x2292, 0x4080, 0x9F, 0x42, 0x40, 0x66, 0x4E, 0x70, 0xF8, 0x59);

      internal static _tagpropertykey WPD_CLIENT_NAME = new _tagpropertykey {fmtid = ClientGuid, pid = 2};

      internal static _tagpropertykey WPD_CLIENT_MAJOR_VERSION = new _tagpropertykey {fmtid = ClientGuid, pid = 3};

      internal static _tagpropertykey WPD_CLIENT_MINOR_VERSION = new _tagpropertykey {fmtid = ClientGuid, pid = 4};

      internal static _tagpropertykey WPD_CLIENT_REVISION = new _tagpropertykey {fmtid = ClientGuid, pid = 5};

      #endregion // Client Properties


      #region Device Properties

      /// <summary>The generic ID of a portable device. </summary>
      internal const string WPD_DEVICE_OBJECT_ID = "DEVICE";

      /// <summary>A human-readable description of a device's synchronization partner. This is a device, application, or server that the device communicates with to maintain a common state or group of files between both partners.</summary>
      private static readonly Guid WPD_DEVICE_SYNC_PARTNER = new Guid(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC);

      /// <summary>The firmware version for the device.</summary>
      internal static _tagpropertykey WPD_DEVICE_FIRMWARE_VERSION = new _tagpropertykey {fmtid = WPD_DEVICE_SYNC_PARTNER, pid = 3};

      /// <summary>A value from 0 to 100 that indicates the power level of the device battery, with 0 being none, and 100 being fully charged.</summary>
      internal static _tagpropertykey WPD_DEVICE_POWER_LEVEL = new _tagpropertykey {fmtid = WPD_DEVICE_SYNC_PARTNER, pid = 4};

      /// <summary>A WPD_POWER_SOURCES enumeration that indicates the power source of the device.</summary>
      internal static _tagpropertykey WPD_DEVICE_POWER_SOURCE = new _tagpropertykey {fmtid = WPD_DEVICE_SYNC_PARTNER, pid = 5};

      /// <summary>The device protocol that is being used.</summary>
      internal static _tagpropertykey WPD_DEVICE_PROTOCOL = new _tagpropertykey {fmtid = WPD_DEVICE_SYNC_PARTNER, pid = 6};

      /// <summary>The serial number of the device.</summary>
      internal static _tagpropertykey WPD_DEVICE_SERIAL_NUMBER = new _tagpropertykey {fmtid = WPD_DEVICE_SYNC_PARTNER, pid = 9};

      /// <summary>Indicates the device type. Applications use this property for representation purposes only. Functional characteristics of the device are decided through functional objects.</summary>
      internal static _tagpropertykey WPD_DEVICE_TYPE = new _tagpropertykey {fmtid = WPD_DEVICE_SYNC_PARTNER, pid = 15};

      internal static _tagpropertykey WPD_DEVICE_TRANSPORT = new _tagpropertykey {fmtid = new Guid(0x463DD662, 0x7FC4, 0x4291, 0x91, 0x1C, 0x7F, 0x4C, 0x9C, 0xCA, 0x97, 0x99), pid = 4};

      #endregion // Device Properties


      #region Object Properties

      /// <summary>ObjectGuid</summary>
      private static readonly Guid ObjectGuid = new Guid(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C);

      ///// <summary>A string ID that uniquely identifies the object on the device. This ID need not be stored across sessions.</summary>
      //internal static _tagpropertykey WPD_OBJECT_ID = new _tagpropertykey {fmtid = ObjectGuid, pid = 2}; // WPD_OBJECT_ID

      /// <summary>The object ID of the parent object. The only object that can return an empty string for this value is the root device object.</summary>
      internal static _tagpropertykey WPD_OBJECT_PARENT_ID = new _tagpropertykey {fmtid = ObjectGuid, pid = 3};

      /// <summary>The display name for the object.</summary>
      internal static _tagpropertykey WPD_OBJECT_NAME = new _tagpropertykey {fmtid = ObjectGuid, pid = 4};

      ///// <summary>A string ID that uniquely identifies the object on the device, similar to <see cref="T:WpdObjectId"/>, but it must be stored across sessions.</summary>
      //internal static _tagpropertykey WPD_OBJECT_PERSISTENT_UNIQUE_ID = new _tagpropertykey {fmtid = ObjectGuid, pid = 5};

      ///// <summary>A <see cref="Guid"/> identifying the format of the object data. This can be a format defined by Windows Portable Devices, or a custom driver format.</summary>
      //internal static _tagpropertykey WPD_OBJECT_FORMAT = new _tagpropertykey {fmtid = ObjectGuid, pid = 6};

      /// <summary>A GUID identifying the generic type of this object, for example, a document or e-mail. This can be an object type defined by Windows Portable Devices, or a custom driver content type. The device object is the only object that does not report this property.</summary>
      internal static _tagpropertykey WPD_OBJECT_CONTENT_TYPE = new _tagpropertykey {fmtid = ObjectGuid, pid = 7};

      ///// <summary>Indicates whether the object should be hidden. If not present, the object is assumed to be not hidden.</summary>
      //internal static _tagpropertykey WPD_OBJECT_ISHIDDEN = new _tagpropertykey {fmtid = ObjectGuid, pid = 9};

      ///// <summary>Indicates whether the object represents system data (such as a system file). If not present, the object is assumed to be not a system object.</summary>
      //internal static _tagpropertykey WPD_OBJECT_ISSYSTEM = new _tagpropertykey {fmtid = ObjectGuid, pid = 10};

      /// <summary>The size of the object resource data.</summary>
      internal static _tagpropertykey WPD_OBJECT_SIZE = new _tagpropertykey {fmtid = ObjectGuid, pid = 11};

      /// <summary>A string name for the file.</summary>
      internal static _tagpropertykey WPD_OBJECT_ORIGINAL_FILE_NAME = new _tagpropertykey {fmtid = ObjectGuid, pid = 12};

      ///// <summary>Determines whether this object is intended to be understood or merely stored by the device. If this property is not present, all data is assumed to be intended for consumption.</summary>
      //internal static _tagpropertykey WPD_OBJECT_NON_CONSUMABLE = new _tagpropertykey {fmtid = ObjectGuid, pid = 13};

      ///// <summary>An IPortableDevicePropVariantCollection containing a collection of string object IDs identifying the referenced objects. This is required only if the object is a reference object, such as a folder or playlist.</summary>
      //internal static _tagpropertykey WPD_OBJECT_REFERENCES = new _tagpropertykey {fmtid = ObjectGuid, pid = 14};

      ///// <summary>String containing a list of space-delimited keywords associated with this object.</summary>
      //internal static _tagpropertykey WPD_OBJECT_KEYWORDS = new _tagpropertykey {fmtid = ObjectGuid, pid = 15};

      ///// <summary>An opaque string created by a client to retain state between sessions without retaining a catalogue of connected device content.</summary>
      //internal static _tagpropertykey WPD_OBJECT_SYNC_ID = new _tagpropertykey {fmtid = ObjectGuid, pid = 16};

      ///// <summary>Indicates whether the media data is DRM-protected. If not present, this is assumed to be False.</summary>
      //[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Drm")]
      //internal static _tagpropertykey WPD_OBJECT_IS_DRM_PROTECTED = new _tagpropertykey {fmtid = ObjectGuid, pid = 17};

      ///// <summary>Indicates the date and time the object was created on the device.</summary>
      //internal static _tagpropertykey WPD_OBJECT_DATE_CREATED = new _tagpropertykey {fmtid = ObjectGuid, pid = 18};

      ///// <summary>Indicates the date and time the object was modified on the device.</summary>
      //internal static _tagpropertykey WPD_OBJECT_DATE_MODIFIED = new _tagpropertykey {fmtid = ObjectGuid, pid = 19};

      ///// <summary>The object ID of the closest functional object containing this object. For example, a file inside a storage functional object will have this property set to the ID of the storage functional object.</summary>
      //internal static _tagpropertykey WPD_OBJECT_CONTAINER_FUNCTIONAL_OBJECT_ID = new _tagpropertykey {fmtid = ObjectGuid, pid = 23};

      ///// <summary>If an object is a hint location, this property indicates the hint-specific name to display to the user instead of the object name.</summary>
      //internal static _tagpropertykey WPD_OBJECT_HINT_LOCATION_DISPLAY_NAME = new _tagpropertykey {fmtid = ObjectGuid, pid = 25};

      ///// <summary>Indicates whether the object can be deleted, or not.</summary>
      //internal static _tagpropertykey WPD_OBJECT_CAN_DELETE = new _tagpropertykey {fmtid = ObjectGuid, pid = 26};

      #endregion // Object Properties


      #region Storage Properties

      private static readonly Guid StorageGuid = new Guid(0x01A3057A, 0x74D6, 0x4E80, 0xBE, 0xA7, 0xDC, 0x4C, 0x21, 0x2C, 0xE5, 0x0A);

      internal static _tagpropertykey WPD_STORAGE_TYPE = new _tagpropertykey {fmtid = StorageGuid, pid = 2};

      internal static _tagpropertykey WPD_STORAGE_FILE_SYSTEM_TYPE = new _tagpropertykey {fmtid = StorageGuid, pid = 3};

      internal static _tagpropertykey WPD_STORAGE_CAPACITY = new _tagpropertykey {fmtid = StorageGuid, pid = 4};

      internal static _tagpropertykey WPD_STORAGE_FREE_SPACE_IN_BYTES = new _tagpropertykey {fmtid = StorageGuid, pid = 5};

      //internal static _tagpropertykey WPD_STORAGE_FREE_SPACE_IN_OBJECTS = new _tagpropertykey {fmtid = StorageGuid, pid = 6};

      internal static _tagpropertykey WPD_STORAGE_DESCRIPTION = new _tagpropertykey {fmtid = StorageGuid, pid = 7};

      internal static _tagpropertykey WPD_STORAGE_SERIAL_NUMBER = new _tagpropertykey {fmtid = StorageGuid, pid = 8};

      //internal static _tagpropertykey WPD_STORAGE_MAX_OBJECT_SIZE = new _tagpropertykey {fmtid = StorageGuid, pid = 9};

      //internal static _tagpropertykey WPD_STORAGE_CAPACITY_IN_OBJECTS = new _tagpropertykey {fmtid = StorageGuid, pid = 10};

      #endregion // Storage Properties


      #region Content Type

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_APPOINTMENT represents an appointment in a calendar.</summary>
      internal static Guid WPD_CONTENT_TYPE_APPOINTMENT = new Guid(0x0FED060E, 0x8793, 0x4B1E, 0x90, 0xC9, 0x48, 0xAC, 0x38, 0x9A, 0xC6, 0x31);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_AUDIO represents an audio file, such as a Windows Media Audio (WMA) or MP3 file.</summary>
      internal static Guid WPD_CONTENT_TYPE_AUDIO = new Guid(0x4AD2C85E, 0x5E2D, 0x45E5, 0x88, 0x64, 0x4F, 0x22, 0x9E, 0x3C, 0x6C, 0xF0);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_AUDIOAlbum represents a collection of audio files. An audio album is functionally equivalent to a playlist of audio files, but is used to represent a physical album to the user.</summary>
      internal static Guid WPD_CONTENT_TYPE_AUDIO_ALBUM = new Guid(0xAA18737E, 0x5009, 0x48FA, 0xAE, 0x21, 0x85, 0xF2, 0x43, 0x83, 0xB4, 0xE6);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_CALENDAR represents a calendar. The object can be a file that contains calendar information or a folder that contains other calendar-related objects, such as tasks, appointments, and so on.</summary>
      internal static Guid WPD_CONTENT_TYPE_CALENDAR = new Guid(0xA1FD5967, 0x6023, 0x49A0, 0x9D, 0xF1, 0xF8, 0x06, 0x0B, 0xE7, 0x51, 0xB0);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_CERTIFICATE represents a certificate used for authentication. </summary>
      internal static Guid WPD_CONTENT_TYPE_CERTIFICATE = new Guid(0xDC3876E8, 0xA948, 0x4060, 0x90, 0x50, 0xCB, 0xD7, 0x7E, 0x8A, 0x3D, 0x87);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_CONTACT represents personal contact data.</summary>
      internal static Guid WPD_CONTENT_TYPE_CONTACT = new Guid(0xEABA8313, 0x4525, 0x4707, 0x9F, 0x0E, 0x87, 0xC6, 0x80, 0x8E, 0x94, 0x35);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_CONTACTGroup represents a group of contacts. This object's WPD_OBJECT_REFERENCES contains a list of object IDs for various WPD_CONTENT_TYPE_CONTACT objects.</summary>
      internal static Guid WPD_CONTENT_TYPE_CONTACT_GROUP = new Guid(0x346B8932, 0x4C36, 0x40D8, 0x94, 0x15, 0x18, 0x28, 0x29, 0x1F, 0x9D, 0xE9);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_DOCUMENT represents a document. Examples include Microsoft Office Word files, Microsoft Office Excel files, plain text files, and other proprietary document formats.</summary>
      internal static Guid WPD_CONTENT_TYPE_DOCUMENT = new Guid(0x680ADF52, 0x950A, 0x4041, 0x9B, 0x41, 0x65, 0xE3, 0x93, 0x64, 0x81, 0x55);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_EMAIL represents an e-mail.</summary>
      internal static Guid WPD_CONTENT_TYPE_EMAIL = new Guid(0x8038044A, 0x7E51, 0x4F8F, 0x88, 0x3D, 0x1D, 0x06, 0x23, 0xD1, 0x45, 0x33);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_FOLDER represents a folder.</summary>
      internal static Guid WPD_CONTENT_TYPE_FOLDER = new Guid(0x27E2E392, 0xA111, 0x48E0, 0xAB, 0x0C, 0xE1, 0x77, 0x05, 0xA0, 0x5F, 0x85);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_GENERIC_FILE represents a generic object with an underlying physical file. The difference between this type and WPD_CONTENT_TYPE_UNSPECIFIED is that an UNSPECIFIED object is more generic and is not required to have an underlying file. This type of object might be created to hold data of an unspecified type that the device is not meant to consume.</summary>
      internal static Guid WPD_CONTENT_TYPE_GENERIC_FILE = new Guid(0x0085E0A6, 0x8D34, 0x45D7, 0xBC, 0x5C, 0x44, 0x7E, 0x59, 0xC7, 0x3D, 0x48);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_GENERIC_MESSAGE represents a generic object with an underlying physical file. The difference between this type and WPD_CONTENT_TYPE_UNSPECIFIED is that an UNSPECIFIED object is more generic and is not required to have an underlying file. This type of object might be created to hold data of an unspecified type that the device is not meant to consume.</summary>
      internal static Guid WPD_CONTENT_TYPE_GENERIC_MESSAGE = new Guid(0xE80EAAF8, 0xB2DB, 0x4133, 0xB6, 0x7E, 0x1B, 0xEF, 0x4B, 0x4A, 0x6E, 0x5F);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_FUNCTIONAL_OBJECT represents a functional object, encapsulating device functionality.</summary>
      internal static Guid WPD_CONTENT_TYPE_FUNCTIONAL_OBJECT = new Guid(0x99ED0160, 0x17FF, 0x4C44, 0x9D, 0x98, 0x1D, 0x7A, 0x6F, 0x94, 0x19, 0x21);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_IMAGE represents a still image.</summary>
      internal static Guid WPD_CONTENT_TYPE_IMAGE = new Guid(0xef2107d5, 0xa52a, 0x4243, 0xa2, 0x6b, 0x62, 0xd4, 0x17, 0x6d, 0x76, 0x03);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_IMAGEAlbum represents a collection of still images. An image album is functionally equivalent to a playlist of image files, but is used to represent a physical photo album to the end user.</summary>
      internal static Guid WPD_CONTENT_TYPE_IMAGE_ALBUM = new Guid(0x75793148, 0x15F5, 0x4A30, 0xA8, 0x13, 0x54, 0xED, 0x8A, 0x37, 0xE2, 0x26);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_NETWORK_ASSOCIATION represents an association between a host and a device.</summary>
      internal static Guid WPD_CONTENT_TYPE_NETWORK_ASSOCIATION = new Guid(0x031DA7EE, 0x18C8, 0x4205, 0x84, 0x7E, 0x89, 0xA1, 0x12, 0x61, 0xD0, 0xF3);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_MEDIA_CAST represents a collection of related content. </summary>
      internal static Guid WPD_CONTENT_TYPE_MEDIA_CAST = new Guid(0x5E88B3CC, 0x3E65, 0x4E62, 0xBF, 0xFF, 0x22, 0x94, 0x95, 0x25, 0x3A, 0xB0);

      /// <summary>An object that describes its type as WPD_CONTENT_TYPE_MEMO represents a text note.</summary>
      internal static Guid WPD_CONTENT_TYPE_MEMO = new Guid(0x9CD20ECF, 0x3B50, 0x414F, 0xA6, 0x41, 0xE4, 0x73, 0xFF, 0xE4, 0x57, 0x51);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_MIXED_CONTENT_ALBUM represents a collection of mixed media objects, for example, audio, image, and video files. A mixed content album is functionally equivalent to a playlist of media files, but is used to represent a physical album to the end user.</summary>
      internal static Guid WPD_CONTENT_TYPE_MIXED_CONTENT_ALBUM = new Guid(0x00F0C3AC, 0xA593, 0x49AC, 0x92, 0x19, 0x24, 0xAB, 0xCA, 0x5A, 0x25, 0x63);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_PLAYLIST represents a playlist. A playlist can be represented by a physical file, or it can consist of references stored as metadata.</summary>
      internal static Guid WPD_CONTENT_TYPE_PLAYLIST = new Guid(0x28D8D31E, 0x249C, 0x454E, 0xAA, 0xBC, 0x34, 0x88, 0x31, 0x68, 0xE6, 0x34);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_PROGRAM represents an executable program.</summary>
      internal static Guid WPD_CONTENT_TYPE_PROGRAM = new Guid(0xD269F96A, 0x247C, 0x4BFF, 0x98, 0xFB, 0x97, 0xF3, 0xC4, 0x92, 0x20, 0xE6);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_SECTION represents a section of data that is contained in another object. For example, a large audio file can be described as a collection of multiple chapters, and each chapter might be a WPD_CONTENT_TYPE_SECTION object.</summary>
      internal static Guid WPD_CONTENT_TYPE_SECTION = new Guid(0x821089F5, 0x1D91, 0x4DC9, 0xBE, 0x3C, 0xBB, 0xB1, 0xB3, 0x5B, 0x18, 0xCE);

      /// <summary>An object that describes its type as WPD_CONTENT_TYPE_TASK represents a task, such as an item in a to-do list.</summary>
      internal static Guid WPD_CONTENT_TYPE_TASK = new Guid(0x63252F2C, 0x887F, 0x4CB6, 0xB1, 0xAC, 0xD2, 0x98, 0x55, 0xDC, 0xEF, 0x6C);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_UNSPECIFIED represents a generic object that may or may not have an underlying physical file. The difference between this type and WPD_CONTENT_TYPE_GENERIC_FILE is that WPD_CONTENT_TYPE_GENERIC_FILE objects must have an underlying physical file.</summary>
      internal static Guid WPD_CONTENT_TYPE_UNSPECIFIED = new Guid(0x28D8D31E, 0x249C, 0x454E, 0xAA, 0xBC, 0x34, 0x88, 0x31, 0x68, 0xE6, 0x34);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_VIDEO represents a video file.</summary>
      internal static Guid WPD_CONTENT_TYPE_VIDEO = new Guid(0x9261B03C, 0x3D78, 0x4519, 0x85, 0xE3, 0x02, 0xC5, 0xE1, 0xF5, 0x0B, 0xB9);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_VIDEOAlbum represents a collection of video files. A video album is functionally equivalent to a playlist of video files, but is used to represent a physical object to the end user.</summary>
      internal static Guid WPD_CONTENT_TYPE_VIDEO_ALBUM = new Guid(0x012B0DB7, 0xD4C1, 0x45D6, 0xB0, 0x81, 0x94, 0xB8, 0x77, 0x79, 0x61, 0x4F);

      /// <summary>An object that describes its type WPD_CONTENT_TYPE_WIRELESS_PROFILE represents information used to access a wireless network.</summary>
      internal static Guid WPD_CONTENT_TYPE_WIRELESS_PROFILE = new Guid(0x0BAC070A, 0x9F5F, 0x4DA4, 0xA8, 0xF6, 0x3D, 0xE4, 0x4D, 0x68, 0xFD, 0x6C);

      #endregion // Content Type
   }
}
