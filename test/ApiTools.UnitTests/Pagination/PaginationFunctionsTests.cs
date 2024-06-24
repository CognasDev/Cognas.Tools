using Cognas.ApiTools.Pagination;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.ComponentModel;

namespace ApiTools.UnitTests.Pagination;

/// <summary>
/// 
/// </summary>
public sealed class PaginationFunctionsTests
{
    #region Unit Test Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageSize"></param>
    [Theory]
    [InlineData(5)]
    public void TakeQuantity(int pageSize)
    {
        Mock<IPaginationQuery> mockPaginationQuery = new();
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageSize).Returns(pageSize);
        PaginationFunctions paginationFunctions = new();
        paginationFunctions.TakeQuantity(mockPaginationQuery.Object).Should().Be(pageSize);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    [Theory]
    [InlineData(5, 1)]
    public void SkipNumber(int pageSize, int pageNumber)
    {
        Mock<IPaginationQuery> mockPaginationQuery = new();
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageSize).Returns(pageSize);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageNumber).Returns(pageNumber);

        int expectedSkipNumber = (pageNumber - 1) * pageSize;
        PaginationFunctions paginationFunctions = new();
        paginationFunctions.SkipNumber(mockPaginationQuery.Object).Should().Be(expectedSkipNumber);
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void OrderByProperty_Valid()
    {
        Mock<IPaginationQuery> mockPaginationQuery = new();
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.OrderBy).Returns(nameof(TestDto.Name));

        PaginationFunctions paginationFunctions = new();
        PropertyDescriptor orderByProperty = paginationFunctions.OrderByProperty<TestDto>(mockPaginationQuery.Object);
        orderByProperty.Name.Should().Be(nameof(TestDto.Name));
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void OrderByProperty_Invalid()
    {
        string invalidPropertyName = Guid.NewGuid().ToString();
        Mock<IPaginationQuery> mockPaginationQuery = new();
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.OrderBy).Returns(invalidPropertyName);

        Action action = () =>
        {
            PaginationFunctions paginationFunctions = new();
            PropertyDescriptor orderByProperty = paginationFunctions.OrderByProperty<TestDto>(mockPaginationQuery.Object);
        };

        action.Should().Throw<PaginationQueryParametersException>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    [Theory]
    [InlineData(5, 1)]
    public void IsPaginationQueryValidOrDefault_Valid(int pageSize, int pageNumber)
    {
        Mock<IPaginationQuery> mockPaginationQuery = new();
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageSize).Returns(pageSize);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageNumber).Returns(pageNumber);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.OrderBy).Returns(nameof(TestDto.Name));

        PaginationFunctions paginationFunctions = new();
        paginationFunctions.IsPaginationQueryValidOrNotRequested<TestDto>(mockPaginationQuery.Object).Should().BeTrue();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageNumber"></param>
    [Theory]
    [InlineData(1)]
    public void IsPaginationQueryValidOrDefault_Invalid_MissingPageSize(int pageNumber)
    {
        Mock<IPaginationQuery> mockPaginationQuery = new();
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageNumber).Returns(pageNumber);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.OrderBy).Returns(nameof(TestDto.Name));

        PaginationFunctions paginationFunctions = new();
        paginationFunctions.IsPaginationQueryValidOrNotRequested<TestDto>(mockPaginationQuery.Object).Should().BeFalse();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageSize"></param>
    [Theory]
    [InlineData(1)]
    public void IsPaginationQueryValidOrDefault_Invalid_MissingPageNumber(int pageSize)
    {
        Mock<IPaginationQuery> mockPaginationQuery = new();
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageSize).Returns(pageSize);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.OrderBy).Returns(nameof(TestDto.Name));

        PaginationFunctions paginationFunctions = new();
        paginationFunctions.IsPaginationQueryValidOrNotRequested<TestDto>(mockPaginationQuery.Object).Should().BeFalse();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    [Theory]
    [InlineData(5, 1)]
    public void BuildPaginationResponseHeader_Total(int pageSize, int pageNumber)
    {
        const string headerKey = "x-total";
        Mock<IPaginationQuery> mockPaginationQuery = new();
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageSize).Returns(pageSize);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageNumber).Returns(pageNumber);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.OrderBy).Returns(nameof(TestDto.Name));

        List<TestModel> testModels = [new TestModel()];
        HttpContext httpContext = new DefaultHttpContext();
        PaginationFunctions paginationFunctions = new();
        paginationFunctions.BuildPaginationResponseHeader<TestModel>(mockPaginationQuery.Object, testModels, httpContext);

        IHeaderDictionary headers = httpContext.Response.Headers;
        headers.Keys.Should().Contain(headerKey);
        headers[headerKey].Should().Equal(testModels.Count.ToString());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    [Theory]
    [InlineData(5, 1)]
    public void BuildPaginationResponseHeader_PageSize(int pageSize, int pageNumber)
    {
        const string headerKey = "x-page-size";
        Mock<IPaginationQuery> mockPaginationQuery = new();
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageSize).Returns(pageSize);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageNumber).Returns(pageNumber);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.OrderBy).Returns(nameof(TestDto.Name));

        List<TestModel> testModels = [new TestModel()];
        HttpContext httpContext = new DefaultHttpContext();
        PaginationFunctions paginationFunctions = new();
        paginationFunctions.BuildPaginationResponseHeader<TestModel>(mockPaginationQuery.Object, testModels, httpContext);

        IHeaderDictionary headers = httpContext.Response.Headers;
        headers.Keys.Should().Contain(headerKey);
        headers[headerKey].Should().Equal(pageSize.ToString());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    [Theory]
    [InlineData(5, 1)]
    public void BuildPaginationResponseHeader_PageCount(int pageSize, int pageNumber)
    {
        const string headerKey = "x-page-count";
        Mock<IPaginationQuery> mockPaginationQuery = new();
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageSize).Returns(pageSize);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageNumber).Returns(pageNumber);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.OrderBy).Returns(nameof(TestDto.Name));

        List<TestModel> testModels = [new TestModel()];
        HttpContext httpContext = new DefaultHttpContext();
        PaginationFunctions paginationFunctions = new();
        paginationFunctions.BuildPaginationResponseHeader<TestModel>(mockPaginationQuery.Object, testModels, httpContext);

        IHeaderDictionary headers = httpContext.Response.Headers;
        int pageCount = (testModels.Count - 1) / pageSize + 1;
        headers.Keys.Should().Contain(headerKey);
        headers[headerKey].Should().Equal(pageCount.ToString());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    [Theory]
    [InlineData(5, 1)]
    public void BuildPaginationResponseHeader_CurrentPage(int pageSize, int pageNumber)
    {
        const string headerKey = "x-page-number";
        Mock<IPaginationQuery> mockPaginationQuery = new();
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageSize).Returns(pageSize);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageNumber).Returns(pageNumber);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.OrderBy).Returns(nameof(TestDto.Name));

        List<TestModel> testModels = [new TestModel()];
        HttpContext httpContext = new DefaultHttpContext();
        PaginationFunctions paginationFunctions = new();
        paginationFunctions.BuildPaginationResponseHeader<TestModel>(mockPaginationQuery.Object, testModels, httpContext);

        IHeaderDictionary headers = httpContext.Response.Headers;
        headers.Keys.Should().Contain(headerKey);
        headers[headerKey].Should().Equal(pageNumber.ToString());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    [Theory]
    [InlineData(5, 1)]
    public void BuildPaginationResponseHeader_OrderBy(int pageSize, int pageNumber)
    {
        const string headerKey = "x-page-orderby";
        Mock<IPaginationQuery> mockPaginationQuery = new();
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageSize).Returns(pageSize);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageNumber).Returns(pageNumber);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.OrderBy).Returns(nameof(TestDto.Name));

        List<TestModel> testModels = [new TestModel()];
        HttpContext httpContext = new DefaultHttpContext();
        PaginationFunctions paginationFunctions = new();
        paginationFunctions.BuildPaginationResponseHeader<TestModel>(mockPaginationQuery.Object, testModels, httpContext);

        IHeaderDictionary headers = httpContext.Response.Headers;
        headers.Keys.Should().Contain(headerKey);
        headers[headerKey].Should().Equal(nameof(TestDto.Name));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    /// <param name="orderByAscending"></param>
    [Theory]
    [InlineData(5, 1, true)]
    [InlineData(5, 1, false)]
    public void BuildPaginationResponseHeader_OrderByAscending(int pageSize, int pageNumber, bool orderByAscending)
    {
        const string headerKey = "x-page-orderbyascending";
        Mock<IPaginationQuery> mockPaginationQuery = new();
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageSize).Returns(pageSize);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.PageNumber).Returns(pageNumber);
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.OrderBy).Returns(nameof(TestDto.Name));
        mockPaginationQuery.SetupGet(paginationQuery => paginationQuery.OrderByAscending).Returns(orderByAscending);
        List<TestModel> testModels = [new TestModel()];
        HttpContext httpContext = new DefaultHttpContext();
        PaginationFunctions paginationFunctions = new();
        paginationFunctions.BuildPaginationResponseHeader<TestModel>(mockPaginationQuery.Object, testModels, httpContext);

        IHeaderDictionary headers = httpContext.Response.Headers;
        headers.Keys.Should().Contain(headerKey);
        headers[headerKey].Should().Equal(orderByAscending.ToString().ToLowerInvariant());
    }

    #endregion

    #region Test Helper Classes

    /// <summary>
    /// 
    /// </summary>
    private sealed record TestDto
    {
        #region Property Declarations

        /// <summary>
        /// 
        /// </summary>
        public string? Name { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    private sealed record TestModel
    {
        #region Property Declarations

        /// <summary>
        /// 
        /// </summary>
        public string? Name { get; set; }

        #endregion
    }

    #endregion
}