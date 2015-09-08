using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//custom
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;
namespace UmbBook.pocos
{
    [TableName("FriendRequests")]
    [PrimaryKey("FriendRequestId", autoIncrement = true)]
    [ExplicitColumns]
    public class FriendRequest
    {
        [Column("FriendRequestId")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int FriendRequestId { get; set; }

        [Column("RequestingUserId")]
        public int RequestingUserId { get; set; }

        [Column("TargetUserId")]
        public int TargetUserId { get; set; }

        [Column("Accepted")]
        public bool Accepted { get; set; }
        
    }
}