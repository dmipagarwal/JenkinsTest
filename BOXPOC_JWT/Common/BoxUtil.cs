using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Box.V2;
using Box.V2.Config;
using Box.V2.JWTAuth;
using Box.V2.Models;
using Nito.AsyncEx;
namespace BOXPOC_JWT.Common
{
    public class BoxUtil
    {
        private bool offline = false;        
        private readonly BoxClient client;
        private static string privateKey = "-----BEGIN RSA PRIVATE KEY-----\n" +
                              "MIIEowIBAAKCAQEAwwJAcIBn1504bohNmL3EvsDMDvloebHHtA0lOybQ8+vO27RI\n" +
                              "yLsChot/7K6D9gRxLjZ3K+nYiVE3wNHYFSrhsG7ph0trDddZjVbtAHxjDrZ0HSz7\n" +
                              "SrP8EzBDxc32yIem839cGw0r2iD6/tkIJR0MFNn8SVWyb82m2k3GwzLOnp0i0irv\n" +
                              "QnbhlI6QmOlp+FA37wo/zaNIKDB9SHbwjdtggnOVnmf03lOhPjGOV2UPuM1Qg4kt\n" +
                              "MY2Z6ebc7MUJzkLdklPgnvN+cnGGZap+9L2aqjmSzzmpHpZyqk5gsc7ri5h7QJUw\n" +
                              "DLtfH6h0FywjS1xltoBN/KxSyunM3nCAvhZ0KwIDAQABAoIBAC6SxxkXUbWpEuUz\n" +
                              "IHUuPWLhYNrirhUCZJOX4CB0cMsQsmK+d/OY1k2T24WHWHTVMsK6t1OBkfmZsBsZ\n" +
                              "AuZrS2N72cI6POMdX4HmFMxc6LuGz8x4BbwdqEJc8JK5UgsqerEE5daAGs0Ju2hK\n" +
                              "JSvX+B6ywRIyqV8SWpsgoCvWYop2yhiTlKZ4a/YFFGNpU0ydEhpIZ5EJCNIcMU/P\n" +
                              "9P+XF+a+mz6VuP4mPakE7L3toiS6O6ZAcelmIr1vMGeqig+tqC7Z7BzMJid7bHnD\n" +
                              "UElubaFunLN26TYZcnq2AvJcNJR9wht3PotQIrCthTsqUKvlKrJVhoPqMStw1eBI\n" +
                              "xMDnpmECgYEA6cWizhCSrGfZIDa/Uedh8GmMQDu9eGak/su14MLr7RQ7JdAOwjFP\n" +
                              "nE+bW22gJsBUDENipJ0l6cJj4FKAdPbJgxrFZgxZ0gFufcJntyPVK+SV0LGxcP9Q\n" +
                              "jfkPQ44FeawSrKSYTcAnIvLPXFSPreY/SXxL8tJumagis3LJ+/JrPT8CgYEA1Y0P\n" +
                              "eD1B60Wu7DiXw5LKDlo05ADYXpacYfZ8SfYMLg1afVRA3hYINZ5mvQouG+V+sMeC\n" +
                              "UbmhQg33FRbqdzv34jYZY3/R0tnLKcmq5ny6yGdcek/6w4Wdz/NhkIowC5sNUiek\n" +
                              "yrR+Bh9eEQKq63J+z0xSss7mK2ctZjVv36QEEhUCgYAIqDwGO5Di95mP2tcs9k+n\n" +
                              "MpFyE4RJwCteEDc1EqLgDb6/ALx7Lo5PMJeFREMJDFhQd/JgPD2aMYu3U/bT3gvK\n" +
                              "YiRrSgvNfiYUXC2xF4+eZ4Gwz9PNZncaOt3413qWbevnHtVRE8ufPLS3K56ChyNS\n" +
                              "2lqJrIdSA2r9kEOwo+KaywKBgEFa92YMEVWIsP0YYNH45Z+42cEBxTZFj7yna+hS\n" +
                              "xe1+Jrh+hY+yXHbUcIrgR7Y/6SL/HBIY5pJJpdmtdrpg/v8bIXADGVsXTocOciAy\n" +
                              "AhP9Fh9UxOD7zo1q5ewPbp7OqCgBe/yRepQzG13HXDnMg6S7rf+NNIIKBKnoJsHh\n" +
                              "92D1AoGBAM3/5i2QP2QN8a2+DiM6Mj9k3f0eMCEaOt6VrrvVVm8uhaYO9S+gyjB4\n" +
                              "lwuRsK3lSc9OILrcRRdJzBliUpnigHdxOE3oSwoH9IX/3Ko4zvW1kmizC/YFbWwj\n" +
                              "FayM2R8/dIm+zritaCqTFeA0ogxPv+OHhFgbFw3fPFWrdromWM90\n" +
                              "-----END RSA PRIVATE KEY-----\n";

        public bool Offline { get { return offline; } }
        public BoxUtil()
        {
            try
            {
                var CLIENT_ID = System.Configuration.ConfigurationManager.AppSettings["CLIENT_ID"].ToString();
                var CLIENT_SECRET = System.Configuration.ConfigurationManager.AppSettings["CLIENT_SECRET"].ToString();
                var ENTERPRISE_ID = System.Configuration.ConfigurationManager.AppSettings["ENTERPRISE_ID"].ToString();
                var JWT_PUBLIC_KEY_ID =  System.Configuration.ConfigurationManager.AppSettings["JWT_PUBLIC_KEY_ID"].ToString();
                var AppUserId = System.Configuration.ConfigurationManager.AppSettings["AppUserId"].ToString();

                var boxConfig = new BoxConfig(CLIENT_ID, CLIENT_SECRET, ENTERPRISE_ID, privateKey, "", JWT_PUBLIC_KEY_ID);
                var boxJWT = new BoxJWTAuth(boxConfig);

                var userToken = boxJWT.UserToken(AppUserId);
                client = boxJWT.UserClient(userToken, AppUserId);
            }
            catch (Exception ex)
            {
                offline = true;
            }

        }
        public List<Parent> GetFolderList(string ParentFolderId)
        {
            if (offline)
            {
                var i = 0;
                return Directory.GetDirectories(ParentFolderId).Select(a =>
                {
                    var info = new DirectoryInfo(a);

                    return new Parent
                    {
                        type = "folder",
                        id = a,
                        sequence_id = i++,
                        etag = info.Name,
                        name = info.Name
                    };
                }).ToList();
            }

            var itemcollection = AsyncContext.Run(() => client.FoldersManager.GetFolderItemsAsync(ParentFolderId, 1000));

            return (from a in itemcollection.Entries
                    where a.Type.ToLower() == "folder"
                    select new Parent
                    {
                        type = a.Type,
                        id = a.Id,
                        sequence_id = a.SequenceId,
                        etag = a.ETag,
                        name = a.Name
                    }).OrderBy(r => r.name).ToList();
        }
        public List<BoxFolderCls> SearchFolder(string ParentFolderId, string search)
        {
            if (offline)
            {
                return Directory.GetFiles(ParentFolderId, search).Select(a =>
                {
                    var info = new FileInfo(a);

                    return new BoxFolderCls
                    {
                        Key = a,
                        Name = info.Name,
                        FileNameWithoutExtension = Path.GetFileNameWithoutExtension(a),
                        Extension = info.Extension,
                        Parentfolderpath = info.Directory.FullName,
                        Size = info.Length,
                        LastModified = info.LastWriteTime
                    };
                }).ToList();
            }

            var itemcollection = AsyncContext.Run(() => client.SearchManager.SearchAsync(search, ancestorFolderIds: new List<string> { ParentFolderId }));

            return (from a in itemcollection.Entries
                    where a.Type.ToLower() == "file"
                    let i = a.Name.LastIndexOf('.')
                    select
                    new BoxFolderCls
                    {
                        Key = a.Id,
                        Name = a.Name,
                        FileNameWithoutExtension = a.Name.Substring(0, i),
                        Extension = a.Name.Substring(i + 1),
                        Parentfolderpath = a.Parent == null ? "" : a.Parent.Id,
                        Size = a.Size ?? 0,
                        LastModified = a.ModifiedAt ?? DateTime.Now
                    }).ToList();
        }

        public List<BoxFolderCls> GetFilesList(string folderId)
        {
            if (offline)
            {
                return Directory.GetFiles(folderId).Select(a =>
                {
                    var info = new FileInfo(a);

                    return new BoxFolderCls
                    {
                        Key = a,
                        Name = info.Name,
                        FileNameWithoutExtension = Path.GetFileNameWithoutExtension(a),
                        Extension = info.Extension,
                        Parentfolderpath = info.Directory.FullName,
                        Size = info.Length,
                        LastModified = info.LastWriteTime
                    };
                }).ToList();
            }

            var itemcollection = AsyncContext.Run(() => client.FoldersManager.GetFolderItemsAsync(folderId, 1000, fields: new List<string> { BoxItem.FieldName, BoxItem.FieldSize, BoxItem.FieldModifiedAt, BoxItem.FieldParent }));

            return (from a in itemcollection.Entries
                    where a.Type.ToLower() == "file"
                    let i = a.Name.LastIndexOf('.')
                    select
                    new BoxFolderCls
                    {
                        Key = a.Id,
                        Name = a.Name,
                        FileNameWithoutExtension = a.Name.Substring(0, i),
                        Extension = a.Name.Substring(i + 1),
                        Parentfolderpath = a.Parent == null ? "" : a.Parent.Id,
                        Size = a.Size ?? 0,
                        LastModified = a.ModifiedAt ?? DateTime.Now
                    }).OrderBy(r => r.Name).ToList();
        }

        public BoxFile UploadFile(string path, string name, string folderID)
        {
            if (offline)
            {
                throw new Exception("Box is offline");
            }

            byte[] byteArray = File.ReadAllBytes(path);
            return UploadFile(byteArray, name, folderID);
        }


        public BoxFile UploadFile(byte[] byteArray, string name, string folderID)
        {
            if (offline)
            {
                throw new Exception("Box is offline");
            }

            var req = new BoxFileRequest
            {
                Name = name,
                ContentCreatedAt = DateTime.Now,
                ContentModifiedAt = DateTime.Now,
                Parent = new BoxRequestEntity { Id = folderID }
            };

            return AsyncContext.Run(() => client.FilesManager.UploadAsync(req, new MemoryStream(byteArray)));
        }

        public Uri PreviewFile(string file)
        {
            if (offline)
            {
                var url = HttpContext.Current.Request.Url;
                return new Uri(url.Scheme + "://" + url.Host + ":" + url.Port + "/Forms/Common/Image.aspx?id=" + file);
            }

            var fileId = ToFileId(file);

            return AsyncContext.Run(() => client.FilesManager.GetPreviewLinkAsync(fileId));
        }

        public Stream ThumbnailFile(string file)
        {
            var fileId = ToFileId(file);
            return AsyncContext.Run(() => client.FilesManager.GetThumbnailAsync(fileId));
        }

        public Stream DownloadFile(string file)
        {
            if (offline)
            {
                return File.OpenRead(file);
            }

            var fileId = ToFileId(file);
            return AsyncContext.Run(() => client.FilesManager.DownloadStreamAsync(fileId));
        }

        public BoxFolderCls FileInfo(string file)
        {
            if (offline)
            {
                var info = new FileInfo(file);
                return new BoxFolderCls
                {
                    Key = file,
                    Name = info.Name,
                    FileNameWithoutExtension = Path.GetFileNameWithoutExtension(file),
                    Extension = info.Extension,
                    Parentfolderpath = info.Directory.FullName,
                    Size = info.Length,
                    LastModified = info.LastWriteTime
                };
            }

            var fileId = ToFileId(file);
            var boxfile = AsyncContext.Run(() => client.FilesManager.GetInformationAsync(fileId));
            var i = boxfile.Name.LastIndexOf('.');
            return new BoxFolderCls
            {
                Key = boxfile.Id,
                Name = boxfile.Name,
                FileNameWithoutExtension = boxfile.Name.Substring(0, i),
                Extension = boxfile.Name.Substring(i + 1),
                Parentfolderpath = boxfile.Parent == null ? "" : boxfile.Parent.Id,
                Size = boxfile.Size ?? 0,
                LastModified = boxfile.ModifiedAt ?? DateTime.Now
            };
        }

        public Uri DownloadUrl(string file)
        {
            if (offline)
            {
                var url = HttpContext.Current.Request.Url;
                return new Uri(url.Scheme + "://" + url.Host + ":" + url.Port + "/Forms/Common/Download.aspx?id=" + file);
            }

            var fileId = ToFileId(file);
            return AsyncContext.Run(() => client.FilesManager.GetDownloadUriAsync(fileId));
        }

        public bool DeleteFile(string file)
        {
            if (offline)
            {
                throw new Exception("Box is offline");
            }

            var fileId = ToFileId(file);
            return AsyncContext.Run(() => client.FilesManager.DeleteAsync(fileId));
        }

        public BoxFile CopyFile(string file, string parentId)
        {
            if (offline)
            {
                throw new Exception("Box is offline");
            }

            var fileId = ToFileId(file);
            var boxFile = AsyncContext.Run(() => client.FilesManager.GetInformationAsync(fileId));

            var req = new BoxFileRequest
            {
                Id = fileId,
                Name = boxFile.Name,
                Parent = new BoxRequestEntity { Id = parentId }
            };

            return AsyncContext.Run(() => client.FilesManager.CopyAsync(req));
        }

        public BoxFile RenameFile(string file, string name)
        {
            if (offline)
            {
                throw new Exception("Box is offline");
            }

            var fileId = ToFileId(file);
            var req = new BoxFileRequest
            {
                Id = fileId,
                Name = name
            };

            return AsyncContext.Run(() => client.FilesManager.UpdateInformationAsync(req));
        }

        private string ToFileId(string fileId)
        {
            return fileId;
        }
        public BoxFolder CreateFolder(string name, string parentId)
        {
            if (offline)
            {
                throw new Exception("Box is offline");
            }

            var req = new BoxFolderRequest
            {
                Name = name,
                Parent = new BoxRequestEntity() { Id = parentId }
            };

            return AsyncContext.Run(() => client.FoldersManager.CreateAsync(req));
        }
    }
}