using GraduateThesis.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace GraduateThesis.Generics
{
    public class ApiControllerBase<TSubRepository, TInput, TOutput, T_ID> : ControllerBase
        where TSubRepository : class
        where TInput : class
        where TOutput : class
        where T_ID : IConvertible
    {
        private readonly TSubRepository _subRepository;
        private Type _subRepositoryType;

        public ApiControllerBase(TSubRepository subRepository)
        {
            _subRepository = subRepository;
            _subRepositoryType = _subRepository.GetType();
        }

        [NonAction]
        protected IActionResult GetActionResult(Exception ex)
        {
            return StatusCode(500, new { Message = $"Exception" });
        }

        [NonAction]
        protected IActionResult GetActionResult(DataResponse dataResponse)
        {
            if (dataResponse.Status == DataResponseStatus.Success)
                return Ok(dataResponse);
            else if (dataResponse.Status == DataResponseStatus.NotFound)
                return NotFound(dataResponse);
            else if (dataResponse.Status == DataResponseStatus.AlreadyExists)
                return StatusCode(409, dataResponse);
            else
                return BadRequest(dataResponse);
        }

        [NonAction]
        protected async Task<IActionResult> GetActionResultAsync<TResponseModel>(string methodName, params object[] parameters)
            //where TResponseModel : List<TOutput>, TOutput, DataResponse, DataResponse<TOutput>
            where TResponseModel : class
        {
            MethodInfo methodInfo = _subRepositoryType.GetMethod(methodName);

            if (methodInfo == null)
                return StatusCode(500);

            Task<TResponseModel> resultAsync = (Task<TResponseModel>)methodInfo.Invoke(_subRepository, parameters)!;
            TResponseModel output = await resultAsync;

            if (output.GetType().Equals(typeof(List<TOutput>)))
            {
                return Ok(output);
            }
            else if (output.GetType().Equals(typeof(TOutput)))
            {
                if (output == null)
                    return NotFound();

                return Ok(output);
            }
            else if (output.GetType().Equals(typeof(DataResponse)))
            {
                DataResponse dataResponse = (DataResponse)Convert.ChangeType(output, typeof(DataResponse));
                return GetActionResult(dataResponse);
            }
            else if (output.GetType().Equals(typeof(DataResponse<TOutput>)))
            {
                DataResponse<TOutput> dataResponse = (DataResponse<TOutput>)Convert.ChangeType(output, typeof(DataResponse));
                return GetActionResult(dataResponse);
            }
            else
                return StatusCode(500);
        }


        [Route("get-list")]
        [HttpGet]
        public virtual async Task<IActionResult> GetList()
        {
            try
            {
                return await GetActionResultAsync<List<TOutput>>("GetListAsync", new object[] { 300 });
            }
            catch (Exception ex)
            {
                return GetActionResult(ex);
            }
        }

        [Route("get-details/{id}")]
        [HttpGet]
        public virtual async Task<IActionResult> GetDetails(T_ID id)
        {
            try
            {
                return await GetActionResultAsync<TOutput>("GetAsync", new object[] { id });
            }
            catch (Exception ex)
            {
                return GetActionResult(ex);
            }
        }

        [Route("create")]
        [HttpPost]
        public virtual async Task<IActionResult> Create(TInput input)
        {
            try
            {
                return await GetActionResultAsync<DataResponse<TOutput>>("CreateAsync", new object[] { input });
            }
            catch (Exception ex)
            {
                return GetActionResult(ex);
            }
        }

        [Route("update/{id}")]
        [HttpPut]
        public virtual async Task<IActionResult> Update(T_ID id, [FromBody] TInput input)
        {
            try
            {
                return await GetActionResultAsync<DataResponse<TOutput>>("UpdateAsync", new object[] { id, input });
            }
            catch (Exception ex)
            {
                return GetActionResult(ex);
            }
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public virtual async Task<IActionResult> BatchDelete(int id)
        {
            try
            {
                return await GetActionResultAsync<DataResponse<TOutput>>("BatchDeleteAsync", new object[] { id });
            }
            catch (Exception ex)
            {
                return GetActionResult(ex);
            }
        }
    }
}
