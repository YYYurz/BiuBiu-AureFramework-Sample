// automatically generated by the FlatBuffers compiler, do not modify


#ifndef FLATBUFFERS_GENERATED_DTENTITY_GAMECONFIG_H_
#define FLATBUFFERS_GENERATED_DTENTITY_GAMECONFIG_H_

#include "flatbuffers/flatbuffers.h"

#include "FP_generated.h"

namespace GameConfig {

struct DTEntity;

struct DTEntityList;

struct DTEntity FLATBUFFERS_FINAL_CLASS : private flatbuffers::Table {
  enum FlatBuffersVTableOffset FLATBUFFERS_VTABLE_UNDERLYING_TYPE {
    VT_ID = 4,
    VT_ASSETNAME = 6
  };
  uint32_t Id() const {
    return GetField<uint32_t>(VT_ID, 0);
  }
  const flatbuffers::String *AssetName() const {
    return GetPointer<const flatbuffers::String *>(VT_ASSETNAME);
  }
  bool Verify(flatbuffers::Verifier &verifier) const {
    return VerifyTableStart(verifier) &&
           VerifyField<uint32_t>(verifier, VT_ID) &&
           VerifyOffset(verifier, VT_ASSETNAME) &&
           verifier.VerifyString(AssetName()) &&
           verifier.EndTable();
  }
};

struct DTEntityBuilder {
  flatbuffers::FlatBufferBuilder &fbb_;
  flatbuffers::uoffset_t start_;
  void add_Id(uint32_t Id) {
    fbb_.AddElement<uint32_t>(DTEntity::VT_ID, Id, 0);
  }
  void add_AssetName(flatbuffers::Offset<flatbuffers::String> AssetName) {
    fbb_.AddOffset(DTEntity::VT_ASSETNAME, AssetName);
  }
  explicit DTEntityBuilder(flatbuffers::FlatBufferBuilder &_fbb)
        : fbb_(_fbb) {
    start_ = fbb_.StartTable();
  }
  DTEntityBuilder &operator=(const DTEntityBuilder &);
  flatbuffers::Offset<DTEntity> Finish() {
    const auto end = fbb_.EndTable(start_);
    auto o = flatbuffers::Offset<DTEntity>(end);
    return o;
  }
};

inline flatbuffers::Offset<DTEntity> CreateDTEntity(
    flatbuffers::FlatBufferBuilder &_fbb,
    uint32_t Id = 0,
    flatbuffers::Offset<flatbuffers::String> AssetName = 0) {
  DTEntityBuilder builder_(_fbb);
  builder_.add_AssetName(AssetName);
  builder_.add_Id(Id);
  return builder_.Finish();
}

inline flatbuffers::Offset<DTEntity> CreateDTEntityDirect(
    flatbuffers::FlatBufferBuilder &_fbb,
    uint32_t Id = 0,
    const char *AssetName = nullptr) {
  auto AssetName__ = AssetName ? _fbb.CreateString(AssetName) : 0;
  return GameConfig::CreateDTEntity(
      _fbb,
      Id,
      AssetName__);
}

struct DTEntityList FLATBUFFERS_FINAL_CLASS : private flatbuffers::Table {
  enum FlatBuffersVTableOffset FLATBUFFERS_VTABLE_UNDERLYING_TYPE {
    VT_DATA = 4
  };
  const flatbuffers::Vector<flatbuffers::Offset<DTEntity>> *data() const {
    return GetPointer<const flatbuffers::Vector<flatbuffers::Offset<DTEntity>> *>(VT_DATA);
  }
  bool Verify(flatbuffers::Verifier &verifier) const {
    return VerifyTableStart(verifier) &&
           VerifyOffset(verifier, VT_DATA) &&
           verifier.VerifyVector(data()) &&
           verifier.VerifyVectorOfTables(data()) &&
           verifier.EndTable();
  }
};

struct DTEntityListBuilder {
  flatbuffers::FlatBufferBuilder &fbb_;
  flatbuffers::uoffset_t start_;
  void add_data(flatbuffers::Offset<flatbuffers::Vector<flatbuffers::Offset<DTEntity>>> data) {
    fbb_.AddOffset(DTEntityList::VT_DATA, data);
  }
  explicit DTEntityListBuilder(flatbuffers::FlatBufferBuilder &_fbb)
        : fbb_(_fbb) {
    start_ = fbb_.StartTable();
  }
  DTEntityListBuilder &operator=(const DTEntityListBuilder &);
  flatbuffers::Offset<DTEntityList> Finish() {
    const auto end = fbb_.EndTable(start_);
    auto o = flatbuffers::Offset<DTEntityList>(end);
    return o;
  }
};

inline flatbuffers::Offset<DTEntityList> CreateDTEntityList(
    flatbuffers::FlatBufferBuilder &_fbb,
    flatbuffers::Offset<flatbuffers::Vector<flatbuffers::Offset<DTEntity>>> data = 0) {
  DTEntityListBuilder builder_(_fbb);
  builder_.add_data(data);
  return builder_.Finish();
}

inline flatbuffers::Offset<DTEntityList> CreateDTEntityListDirect(
    flatbuffers::FlatBufferBuilder &_fbb,
    const std::vector<flatbuffers::Offset<DTEntity>> *data = nullptr) {
  auto data__ = data ? _fbb.CreateVector<flatbuffers::Offset<DTEntity>>(*data) : 0;
  return GameConfig::CreateDTEntityList(
      _fbb,
      data__);
}

}  // namespace GameConfig

#endif  // FLATBUFFERS_GENERATED_DTENTITY_GAMECONFIG_H_
