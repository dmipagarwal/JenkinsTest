using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BOXPOC_JWT.Common
{
    public class BoxCls
    {

    }
    public class FileVersion
    {
        public string type { get; set; }
        public string id { get; set; }
        public string sha1 { get; set; }
    }

    public class Entry2
    {
        public string type { get; set; }
        public string id { get; set; }
        public object sequence_id { get; set; }
        public object etag { get; set; }
        public string name { get; set; }
    }

    public class PathCollection
    {
        public int total_count { get; set; }
        public List<Entry2> entries { get; set; }
    }

    public class CreatedBy
    {
        public string type { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string login { get; set; }
    }

    public class ModifiedBy
    {
        public string type { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string login { get; set; }
    }

    public class OwnedBy
    {
        public string type { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string login { get; set; }
    }

    public class Parent
    {
        public string type { get; set; }
        public string id { get; set; }
        public object sequence_id { get; set; }
        public object etag { get; set; }
        public string name { get; set; }
    }

    public class Entry
    {
        public string type { get; set; }
        public string id { get; set; }
        public FileVersion file_version { get; set; }
        public string sequence_id { get; set; }
        public string etag { get; set; }
        public string sha1 { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int size { get; set; }
        public PathCollection path_collection { get; set; }
        public string created_at { get; set; }
        public string modified_at { get; set; }
        public object trashed_at { get; set; }
        public object purged_at { get; set; }
        public string content_created_at { get; set; }
        public string content_modified_at { get; set; }
        public CreatedBy created_by { get; set; }
        public ModifiedBy modified_by { get; set; }
        public OwnedBy owned_by { get; set; }
        public object shared_link { get; set; }
        public Parent parent { get; set; }
        public string item_status { get; set; }
    }

    public class BoxRespo
    {
        public int total_count { get; set; }
        public List<Entry> entries { get; set; }
    }   
    public class Entry3
    {
        public string type { get; set; }
        public string id { get; set; }
        public FileVersion file_version { get; set; }
        public string sequence_id { get; set; }
        public string etag { get; set; }
        public string sha1 { get; set; }
        public string name { get; set; }
    }

    public class Order
    {
        public string by { get; set; }
        public string direction { get; set; }
    }

    public class ItemCollection
    {
        public int total_count { get; set; }
        public List<Entry3> entries { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public List<Order> order { get; set; }
    }

    public class BoxFolderResp
    {
        public string type { get; set; }
        public string id { get; set; }
        public string sequence_id { get; set; }
        public string etag { get; set; }
        public string name { get; set; }
        public string created_at { get; set; }
        public string modified_at { get; set; }
        public string description { get; set; }
        public long size { get; set; }
        public PathCollection path_collection { get; set; }
        public CreatedBy created_by { get; set; }
        public ModifiedBy modified_by { get; set; }
        public object trashed_at { get; set; }
        public object purged_at { get; set; }
        public string content_created_at { get; set; }
        public string content_modified_at { get; set; }
        public OwnedBy owned_by { get; set; }
        public object shared_link { get; set; }
        public object folder_upload_email { get; set; }
        public Parent parent { get; set; }
        public string item_status { get; set; }
        public ItemCollection item_collection { get; set; }
    }
    public class BoxFolderCls
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string FileNameWithoutExtension { get; set; }
        public string Extension { get; set; }
        public string Parentfolderpath { get; set; }
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
        //public byte[] S3Data { get; set; }
    }
    public class BoxTreeFolderCls
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string FileNameWithoutExtension { get; set; }
        public string Extension { get; set; }
        public string Parentfolderpath { get; set; }
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
        //public byte[] S3Data { get; set; }
    }
}   