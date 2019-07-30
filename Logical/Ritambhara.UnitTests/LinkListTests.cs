using Xunit;

namespace Ritambhara.UnitTests
{
    public class LinkListTests
    {
        [Fact]
        public void GetMiddleElement1()
        {
            Node linkedList = LinkListExample.CreateLinkList(11);
            Node middleElement = LinkListExample.GetMiddleElement(linkedList);

            Assert.Equal(this.GetMiddleNode(linkedList, 11).Data, middleElement.Data);
        }

        [Fact]
        public void GetMiddleElement2()
        {
            Node linkedList = LinkListExample.CreateLinkList(10);
            Node middleElement = LinkListExample.GetMiddleElement(linkedList);

            Assert.Equal(this.GetMiddleNode(linkedList, 10).Data, middleElement.Data);
        }

        private Node GetMiddleNode(Node list, int listCount)
        {
            int middle = listCount / 2;
            Node middleNode = list;

            for (int i = 0; i < middle; i++)
                middleNode = middleNode.Next;

            return middleNode;
        }
    }
}