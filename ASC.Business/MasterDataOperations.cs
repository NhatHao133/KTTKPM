using ASC.Business.Interfaces;
using ASC.DataAccess.Interfaces;
using ASC.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASC.Business
{
    public class MasterDataOperations : ImasterDataOperations
    {
        private readonly IUniOfWork _unitOfWork;

        public MasterDataOperations(IUniOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MasterDataKey>> GetAllMasterKeysAsync()
        {
            var masterKeys = await _unitOfWork.Respository<MasterDataKey>().FindAllAsync();
            return masterKeys.ToList();
        }

        public async Task<List<MasterDataKey>> GetMasterKeyByNameAsync(string name)
        {
            var masterKeys = await _unitOfWork.Respository<MasterDataKey>().FindAllByPartitionKeyAsync(name);
            return masterKeys.ToList();
        }
        public async Task<bool> InsertMasterKeyAsync(MasterDataKey key)
        {
            using (_unitOfWork)
            {
                await _unitOfWork.Respository<MasterDataKey>().AddAsync(key);
                _unitOfWork.CommitTransaction();
                return true;
            }
        }

        public async Task<List<MasterDataValue>> GetAllMasterValuesByKeyAsync(string key)
        {
            try
            {
                var masterKeys = await _unitOfWork.Respository<MasterDataValue>().FindAllByPartitionKeyAsync(key);
                return masterKeys.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<MasterDataValue> GetMasterValueByNameAsync(string key, string name)
        {
            var masterValues = await _unitOfWork.Respository<MasterDataValue>().FindAsync(key, name);
            return masterValues;
        }
        public async Task<bool> InsertMasterValueAsync(MasterDataValue value)
        {
            using (_unitOfWork)
            {
                await _unitOfWork.Respository<MasterDataValue>().AddAsync(value);
                _unitOfWork.CommitTransaction();
                return true;
            }
        }

        public async Task<bool> UpdateMasterKeyAsync(string orginalPartitionKey, MasterDataKey key)
        {
            using (_unitOfWork)
            {
                var masterKey = await _unitOfWork.Respository<MasterDataKey>().FindAsync(orginalPartitionKey, key.RowKey);
                masterKey.IsActive = key.IsActive;
                masterKey.IsDeleted = key.IsDeleted;
                masterKey.Name = key.Name;
                _unitOfWork.Respository<MasterDataKey>().Update(masterKey);
                _unitOfWork.CommitTransaction();
                return true;
            }
        }
        public async Task<bool> UpdateMasterValueAsync(string originalPartitionKey, string originalRowKey, MasterDataValue value)
        {
            using (_unitOfWork)
            {
                var masterValue = await _unitOfWork.Respository<MasterDataValue>().FindAsync(originalPartitionKey, originalRowKey);
                masterValue.IsActive = value.IsActive;
                masterValue.IsDeleted = value.IsDeleted;
                masterValue.Name = value.Name;
                _unitOfWork.Respository<MasterDataValue>().Update(masterValue);
                _unitOfWork.CommitTransaction();
                return true;
            }
        }

        public async Task<List<MasterDataValue>> GetAllMasterValuesAsync()
        {
            var masterValues = await _unitOfWork.Respository<MasterDataValue>().FindAllAsync();
            return masterValues.ToList();
        }
        public async Task<bool> UploadBulkMasterData(List<MasterDataValue> values)
        {
            using (_unitOfWork)
            {
                foreach (var value in values)
                {
                    // Find, if null insert MasterKey
                    var masterKey = await GetMasterKeyByNameAsync(value.PartitionKey);
                    if (!masterKey.Any())
                    {
                        await _unitOfWork.Respository<MasterDataKey>().AddAsync(new MasterDataKey()
                        {
                            Name = value.PartitionKey,
                            RowKey = Guid.NewGuid().ToString(),
                            PartitionKey = value.PartitionKey
                        });
                    }

                    // Find, if null Insert MasterValue
                    var masterValueByKey = await GetAllMasterValuesByKeyAsync(value.PartitionKey);
                    var masterValue = masterValueByKey.FirstOrDefault(p => p.Name == value.Name);
                    if (masterValue == null)
                    {
                        await _unitOfWork.Respository<MasterDataValue>().AddAsync(value);
                    }
                    else
                    {
                        masterValue.IsActive = value.IsActive;
                        masterValue.IsDeleted = value.IsDeleted;
                        masterValue.Name = value.Name;
                        _unitOfWork.Respository<MasterDataValue>().Update(masterValue);
                    }
                }
                _unitOfWork.CommitTransaction();
                return true;
            }
        }
    }
}
