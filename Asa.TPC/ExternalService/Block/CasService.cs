﻿using Asa.TPC.Persistence;
using System;
using System.Threading.Tasks;

namespace Asa.TPC.ExternalService.Block
{
    class CasService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CasService()
        {
            _unitOfWork = new Context();
        }

        public async Task<int> Block(int amount)
        {
            if (amount < 10)
                throw new InvalidOperationException();

            var block = new Domain.Block(amount);
            _unitOfWork.BlockRepository.Save(block);
            _unitOfWork.Commit();

            return block.BlockId;
        }

        public async Task Release(int blockId) 
        {
            var block = await _unitOfWork.BlockRepository.GetBlock(blockId);

            if (block is not null) 
            {
                block.Release();
                _unitOfWork.BlockRepository.Save(block);
                _unitOfWork.Commit();
            }
        }
    }
}
