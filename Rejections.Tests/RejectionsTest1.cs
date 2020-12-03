using System;
using System.Collections.Generic;
using Xunit;

namespace Rejections.Tests
{
    public class RejectionsTest1
    {
        [Fact]
        public void TEST_RECORD_REJECT_NULL_PARAMS(){
            Rejects r = new Rejects(3.0, 15);
            r.Record(null,new DateTime(2008, 5, 1, 8, 30, 52));
            Tuple<DateTime, int> var = r.getRecord(null);
            Assert.True(var==null, "Record is not correct.");
        }
        [Fact]
        public void TEST_RECORD_SYMBOL_NOEXIST()
        {
            Rejects r = new Rejects(3.0, 15);
            r.Record("GOOGL", new DateTime(2008, 5, 1, 8, 30, 52));
            Tuple<DateTime, int> var = r.getRecord("GOOGL");
            Assert.True(var.Equals(new Tuple<DateTime, int>(new DateTime(2008, 5, 1, 8, 30, 52),1)), "Record is not correct.");
        }
        [Fact]
        public void TEST_RECORD_SYMBOL_EXIST()
        {
            Rejects r = new Rejects(3.0,15);
            r.Record("GOOGL", new DateTime(2008, 5, 1, 8, 30, 52));
            r.Record("GOOGL", new DateTime(2007, 5, 1, 8, 30, 52));
            Tuple<DateTime, int> var = r.getRecord("GOOGL");
            Assert.True(var.Equals(new Tuple<DateTime, int>(new DateTime(2007, 5, 1, 8, 30, 52), 2)), "Record is not correct.");
        }
        [Fact]
        public void TEST_RECORD_SYMBOL_IN_IGNORELIST()
        {
            Rejects r = new Rejects(3.0,15);
            r.IgnoreList = new List<String>(){"GOOGL"};
            r.Record("GOOGL", new DateTime(2008, 5, 1, 8, 30, 52));
            Tuple<DateTime, int> var = r.getRecord("GOOGL");
            Assert.True(var == null, "Record is not correct.");
        }
        [Fact]
        public void TEST_ISPERMITTED_REJECT_NULL_PARAMS(){
            Rejects r = new Rejects(3.0, 15);
            bool var = r.isPermitted(null,new DateTime(2008, 5, 1, 8, 30, 52));
            Assert.False(var, "Record is not correct.");
        }
        [Fact]
        public void TEST_ISPERMITTED_TRUE_NULL_IGNORELIST()
        {
            Rejects r = new Rejects(3.0, 15);
            bool var = r.isPermitted("GOOGL",new DateTime(2007, 5, 1, 8, 30, 52));
            Assert.True(var, "Record is not correct.");
        }
        [Fact]
        public void TEST_ISPERMITTED_FALSE_NULL_IGNORELIST()
        {
            Rejects r = new Rejects(3.0, 15);
            r.Record("GOOGL",new DateTime(2007,5,1,8,30,52));
            bool var = r.isPermitted("GOOGL",new DateTime(2007, 5, 1, 8, 30, 52));
            Assert.False(var, "Record is not correct.");
        }
        [Fact]
        public void TEST_ISPERMITTED_IN_IGNORELIST()
        {
            Rejects r = new Rejects(3.0, 15);
            r.IgnoreList = new List<String>(){"GOOGL"};
            r.Record("GOOGL",new DateTime(2007,5,1,8,30,52));
            bool var = r.isPermitted("GOOGL",new DateTime(2007, 5, 1, 8, 30, 52));
            Assert.True(var, "Record is not correct.");
        }
        [Fact]
        public void TEST_ISPERMITTED_SYMBOL_EXISTS_MAX_REJECT()
        {
            Rejects r = new Rejects(3.0, 2);
            r.Record("GOOGL",new DateTime(2007,5,1,8,30,52));
            r.Record("GOOGL",new DateTime(2007,5,1,8,30,53));
            r.Record("GOOGL",new DateTime(2007,5,1,8,30,54));
            bool var = r.isPermitted("GOOGL",new DateTime(2007, 5, 1, 10, 30, 54));
            Assert.False(var, "Record is not correct.");
        }
        [Fact]
        public void TEST_ISPERMITTED_SYMBOL_EXISTS_NEED_WAIT()
        {
            Rejects r = new Rejects(3.0, 15);
            r.Record("GOOGL",new DateTime(2007,5,1,8,30,52));
            bool var = r.isPermitted("GOOGL",new DateTime(2007, 5, 1, 8, 33, 52));
            Assert.False(var, "Record is not correct.");
        }
        [Fact]
        public void TEST_ISPERMITTED_SYMBOL_EXISTS_NO_WAIT()
        {
            Rejects r = new Rejects(3.0, 15);
            r.Record("GOOGL",new DateTime(2007,5,1,8,30,52));
            bool var = r.isPermitted("GOOGL",new DateTime(2007, 5, 1, 8, 33, 53));
            Assert.True(var, "Record is not correct.");
        }
        [Fact]
        public void TEST_ISPERMITTED_SYMBOL_NOEXIST()
        {
            Rejects r = new Rejects(3.0, 15);
            bool var = r.isPermitted("GOOGL",new DateTime(2007, 5, 1, 8, 33, 53));
            Assert.True(var, "Record is not correct.");
        }
    }
}
